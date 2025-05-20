async function CreateHistogram(data, divid) {
    // Set dimensions
    let width = 800;
    let height = 400;
    let margin = { top: 20, right: 30, bottom: 30, left: 40 };

    d3.select(divid).selectAll("*").remove();

    // Create an SVG container
    let histogramsvg = d3.select(divid)
        .attr("viewBox", [0, 0, width, height])
        .attr("style", "max-width: 100%; height: auto; height: intrinsic;");

    // Adjust domain to include negative values
    const xDomain = [d3.min(data), d3.max(data)];

    // Set up a histogram generator
    let bins = d3.histogram()
        .domain(xDomain)
        .thresholds(40)(data);

    // Set scales
    let x = d3.scaleLinear()
        .domain(xDomain)
        .range([margin.left, width - margin.right]);

    let y = d3.scaleLinear()
        .domain([0, d3.max(bins, d => d.length)])
        .range([height - margin.bottom, margin.top]);

    // Draw bars
    histogramsvg.append("g")
        .attr("fill", "steelblue")
        .selectAll("rect")
        .data(bins)
        .join("rect")
        .attr("x", d => x(d.x0) + 1)
        .attr("y", d => y(d.length))
        .attr("width", d => x(d.x1) - x(d.x0) - 1)
        .attr("height", d => y(0) - y(d.length));

    // Draw X-axis
    histogramsvg.append("g")
        .attr("transform", `translate(0,${height - margin.bottom})`)
        .call(d3.axisBottom(x));

    // Draw Y-axis
    histogramsvg.append("g")
        .attr("transform", `translate(${margin.left},0)`)
        .call(d3.axisLeft(y));
}

function AddVaRLine(divid, varValue, label = "VaR") {

    console.log("Adding VaR line to histogram...");
    // Get reference to the existing SVG
    let svg = d3.select(divid);

    // Extract existing scales by re-parsing them (assumes same margins/dimensions)
    let width = 800;
    let height = 400;
    let margin = { top: 20, right: 30, bottom: 30, left: 40 };

    // Get all rect elements to infer domain from bars (or keep xDomain in a shared state)
    let bars = svg.selectAll("rect").data();

    if (!bars || bars.length === 0) {
        console.warn("No bars found. Make sure to call this after CreateHistogram.");
        return;
    }

    // Reconstruct the x scale
    let xDomain = [bars[0].x0, bars[bars.length - 1].x1];
    let x = d3.scaleLinear()
        .domain(xDomain)
        .range([margin.left, width - margin.right]);

    // Draw vertical line at VaR value
    svg.append("line")
        .attr("x1", x(varValue))
        .attr("x2", x(varValue))
        .attr("y1", margin.top)
        .attr("y2", height - margin.bottom)
        .attr("stroke", "red")
        .attr("stroke-width", 2)
        .attr("stroke-dasharray", "4");

    // Add label
    svg.append("text")
        .attr("x", x(varValue) + 5)
        .attr("y", margin.top + 10)
        .attr("fill", "red")
        .style("font-size", "12px")
        .text(label + `: ${varValue.toFixed(2)}`);
}

function CreateHistogramObservable(data, divid) {
    Plot.plot({
        width: 960,
        height: 500,
        marks: [
            Plot.rectY(unemployment, Plot.binX({ y: "count" }, { x: "rate", fill: "steelblue" })),
            Plot.ruleY([0])
        ]
    });
}