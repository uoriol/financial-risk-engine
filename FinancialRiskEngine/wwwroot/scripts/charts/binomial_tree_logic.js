
function GetNodeData(node) {
    let dict = {};
    let optionPrice = "-";
    if (node.optionPrice != null) {
        optionPrice = node.optionPrice.toFixed(4);
    }
    dict.name = node.stockPrice.toFixed(2) + " (" + optionPrice + ")";
    if (node.stepUp != null) {
        dict.children = [];
        dict.children.push(GetNodeData(node.stepUp));
        dict.children.push(GetNodeData(node.stepDown));
    }
    return dict;
}
function CreateBinomialTree(binomialTree, divId) {
    console.log("Here");
    console.log(binomialTree);
    // Data for the tree


    const data = GetNodeData(binomialTree.root);


    const treewidth = 2000;

    // Compute the tree height
    const root = d3.hierarchy(data);
    const dx = 50;
    const dy = treewidth / (root.height + 1);

    // Create a tree layout
    const tree = d3.tree().nodeSize([dx, dy]);

    // Sort and layout
    //root.sort((a, b) => d3.descending(a.data.name, b.data.name));
    tree(root);

    // Compute tree extent
    let x0 = Infinity;
    let x1 = -x0;
    root.each(d => {
        if (d.x > x1) x1 = d.x;
        if (d.x < x0) x0 = d.x;
    });

    const treeheight = x1 - x0 + dx * 2;

    const svgtree = d3.select(divId);
    svgtree.selectAll("*").remove();

    svgtree
        .attr("width", treewidth)
        .attr("height", treeheight)
        .attr("viewBox", [-dy / 3, x0 - dx, treewidth, treeheight])
        .attr("style", "max-width: 100%; height: auto; font: 10px sans-serif;");

    // Add links
    svgtree.append("g")
        .attr("fill", "none")
        .attr("stroke", "#555")
        .attr("stroke-opacity", 0.4)
        .attr("stroke-width", 1.5)
        .selectAll("path")
        .data(root.links())
        .join("path")
        .attr("d", d3.linkHorizontal()
            .x(d => d.y)
            .y(d => d.x));

    // Add nodes
    const node = svgtree.append("g")
        .attr("stroke-linejoin", "round")
        .attr("stroke-width", 3)
        .selectAll("g")
        .data(root.descendants())
        .join("g")
        .attr("transform", d => `translate(${d.y},${d.x})`);

    node.append("circle")
        .attr("fill", d => d.children ? "#555" : "#999")
        .attr("r", 2.5);

    node.append("text")
        .attr("dy", "0.31em")
        .attr("x", d => d.children ? -6 : 6)
        .attr("text-anchor", d => d.children ? "end" : "start")
        .text(d => d.data.name)
        .attr("stroke", "white")
        .attr("paint-order", "stroke")
        .style("cursor", "pointer")
        .style("font-size", "20px")
        .style("fill", d => d.data.color ? d.data.color : "black");

    node.on("click", function (event, d) {
        const nodeName = d.data.name;
        console.log("Node clicked:", nodeName);

        //// Call Blazor method
        DotNet.invokeMethodAsync("FinancialRiskEngine.Client", "OnNodeClicked", nodeName);
    });
    console.log("Finish");
}
