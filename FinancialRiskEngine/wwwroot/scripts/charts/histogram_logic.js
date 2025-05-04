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