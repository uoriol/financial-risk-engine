async function CreatePricesChart(data, divid) {

    data.forEach(d => {
        d.date = new Date(d.date);
        d.value = +d.close;
    });

    // Remove any existing chart before drawing a new one
    d3.select(divid).selectAll("*").remove();

    // Set up dimensions and margins
    const margin = { top: 20, right: 30, bottom: 40, left: 50 };
    const width = 850 - margin.left - margin.right;  // desired drawing area
    const height = 400 - margin.top - margin.bottom;

    // Create an SVG container with full dimensions (including margins)
    let svg = d3.select(divid)
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .attr("viewBox", [0, 0, width + margin.left + margin.right, height + margin.top + margin.bottom])
        .attr("style", "max-width: 100%; height: auto; height: intrinsic;");

    // Create a group for drawing that accounts for the margins
    let g = svg.append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    chartGroup = svg.append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    // Create scales for the x and y axes
    let x = d3.scaleTime()
        .domain(d3.extent(data, d => d.date))  // Get min and max of dates
        .range([0, width]);

    currentMaxY = d3.max(data, d => d.value); // Store the current max value for the y-axis

    let y = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.value)])  // Set y domain from 0 to max value
        .nice()  // Make y-axis round numbers
        .range([height, 0]);

    // Create x-axis and y-axis
    chartGroup.append("g")
        .attr("transform", `translate(0,${height})`)
        .call(d3.axisBottom(x).tickFormat(d3.timeFormat("%b %Y")));  // Customize tick format

    chartGroup.append("g")
        .call(d3.axisLeft(y));

    // Create the line generator
    lineGenerator = d3.line()
        .x(d => x(d.date))
        .y(d => y(d.value))
        .curve(d3.curveMonotoneX);  // Optional: Use smoothing for the line

    // Append the line to the chart for the initial dataset
    chartGroup.append("path")
        .datum(data)
        .attr("class", "line")  // Give a class to the line so we can select it later
        .attr("fill", "none")
        .attr("stroke", "steelblue")
        .attr("stroke-width", 2)
        .attr("d", lineGenerator);
}
function CreateVaRConfidenceLevelChart(data, selector) {

    console.log("Creating VaR Confidence Level Chart", data);

    // Already camelCase, so no need to reformat
    data.forEach(d => {
        d.confidenceLevel = +d.confidenceLevel;
        d.vaR = +d.vaR;
    });

    d3.select(selector).selectAll("*").remove();

    const margin = { top: 20, right: 30, bottom: 50, left: 50 };
    const width = 500 - margin.left - margin.right;
    const height = 400 - margin.top - margin.bottom;

    const svg = d3.select(selector)
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    const x = d3.scaleLinear()
        .domain([0.90, 0.99])
        .range([0, width]);

    const y = d3.scaleLinear()
        .domain([
            d3.min(data, d => d.vaR) * 0.95,
            d3.max(data, d => d.vaR) * 1.05
        ])
        .range([height, 0]);

    svg.append("g")
        .attr("transform", `translate(0, ${height})`)
        .call(d3.axisBottom(x).tickFormat(d3.format(".2f")));

    svg.append("g")
        .call(d3.axisLeft(y));

    const line = d3.line()
        .x(d => x(d.confidenceLevel))
        .y(d => y(d.vaR))
        .curve(d3.curveMonotoneX);

    svg.append("path")
        .datum(data.sort((a, b) => a.confidenceLevel - b.confidenceLevel))
        .attr("fill", "none")
        .attr("stroke", "black")
        .attr("stroke-width", 1.5)
        .attr("d", line);

    svg.append("text")
        .attr("x", width / 2)
        .attr("y", height + margin.bottom - 10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .text("Confidence level");

    svg.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", -margin.left + 15)
        .attr("x", -height / 2)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .text("VaR");
}


function CreateVaRHoldingPeriodChart(data, selector) {

    console.log("Creating VaR Holding Period Chart", data);

    // Already camelCase, so no need to reformat
    data.forEach(d => {
        d.holdingPeriod = +d.holdingPeriod;
        d.vaR = +d.vaR;
    });

    d3.select(selector).selectAll("*").remove();

    const margin = { top: 20, right: 30, bottom: 50, left: 50 };
    const width = 500 - margin.left - margin.right;
    const height = 400 - margin.top - margin.bottom;

    const svg = d3.select(selector)
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", `translate(${margin.left},${margin.top})`);

    const x = d3.scaleLinear()
        .domain(d3.extent(data, d => d.holdingPeriod))
        .range([0, width]);

    const y = d3.scaleLinear()
        .domain([
            d3.min(data, d => d.vaR) * 0.95,
            d3.max(data, d => d.vaR) * 1.05
        ])
        .range([height, 0]);

    svg.append("g")
        .attr("transform", `translate(0, ${height})`)
        .call(d3.axisBottom(x).tickFormat(d3.format(".2f")));

    svg.append("g")
        .call(d3.axisLeft(y));

    const line = d3.line()
        .x(d => x(d.holdingPeriod))
        .y(d => y(d.vaR))
        .curve(d3.curveMonotoneX);

    svg.append("path")
        .datum(data.sort((a, b) => a.holdingPeriod - b.holdingPeriod))
        .attr("fill", "none")
        .attr("stroke", "black")
        .attr("stroke-width", 1.5)
        .attr("d", line);

    svg.append("text")
        .attr("x", width / 2)
        .attr("y", height + margin.bottom - 10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .text("Holding Period");

    svg.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", -margin.left + 15)
        .attr("x", -height / 2)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .text("VaR");
}
