function CreateCandlestrickChartFromPrices(data, divid) {
    //console.log("Initiate chart drawing process");

    //console.log(data);

    //data.forEach(d => {
    //    d.date = new Date(d.date);
    //    d.value = +d.close;
    //});

    //// Remove any existing chart before drawing a new one
    //d3.select(divid).selectAll("*").remove();

    //// Set up dimensions and margins
    //const margin = { top: 20, right: 30, bottom: 40, left: 50 };
    //const width = 850 - margin.left - margin.right;  // desired drawing area
    //const height = 400 - margin.top - margin.bottom;

    //// Create an SVG container with full dimensions (including margins)
    //let svg = d3.select(divid)
    //    .attr("width", width + margin.left + margin.right)
    //    .attr("height", height + margin.top + margin.bottom)
    //    .attr("viewBox", [0, 0, width + margin.left + margin.right, height + margin.top + margin.bottom])
    //    .attr("style", "max-width: 100%; height: auto; height: intrinsic;");

    //// Create a group for drawing that accounts for the margins
    //let g = svg.append("g")
    //    .attr("transform", `translate(${margin.left},${margin.top})`);

    //chartGroup = svg.append("g")
    //    .attr("transform", `translate(${margin.left},${margin.top})`);

    //// Create scales for the x and y axes
    //let x = d3.scaleTime()
    //    .domain(d3.extent(data, d => d.date))  // Get min and max of dates
    //    .range([0, width]);

    //currentMaxY = d3.max(data, d => d.value); // Store the current max value for the y-axis

    //let y = d3.scaleLinear()
    //    .domain([0, d3.max(data, d => d.value)])  // Set y domain from 0 to max value
    //    .nice()  // Make y-axis round numbers
    //    .range([height, 0]);

    //// Create x-axis and y-axis
    //chartGroup.append("g")
    //    .attr("transform", `translate(0,${height})`)
    //    .call(d3.axisBottom(x).tickFormat(d3.timeFormat("%b %Y")));  // Customize tick format

    //chartGroup.append("g")
    //    .call(d3.axisLeft(y));

    //// Create the line generator
    //lineGenerator = d3.line()
    //    .x(d => x(d.date))
    //    .y(d => y(d.value))
    //    .curve(d3.curveMonotoneX);  // Optional: Use smoothing for the line

    //// Append the line to the chart for the initial dataset
    //chartGroup.append("path")
    //    .datum(data)
    //    .attr("class", "line")  // Give a class to the line so we can select it later
    //    .attr("fill", "none")
    //    .attr("stroke", "steelblue")
    //    .attr("stroke-width", 2)
    //    .attr("d", lineGenerator);
    
}

function CreateCandlestickReal() {
    //const container = document.querySelector(divid);
    //container.innerHTML = "";

    //data.forEach(d => {
    //    d.date = new Date(d.date); // parse ISO string to Date object
    //});

    //const chart = Plot.plot({
    //    grid: true,
    //    x: { type: "time", label: "Date" },  // time scale avoids band collisions
    //    y: { label: "Simulated prices" },
    //    color: { domain: [-1, 0, 1], range: ["#e41a1c", "#000000", "#4daf4a"] },
    //    marks: [
    //        Plot.ruleY(data, {
    //            x: "date",
    //            y1: "open",
    //            y2: "close",
    //            stroke: d => Math.sign(d.close - d.open),
    //            strokeWidth: 4,
    //            strokeLinecap: "round"
    //        })
    //    ]
    //});

    //container.appendChild(chart);
    //chart.setAttribute("viewBox", `0 0 ${chart.getAttribute("width")} ${chart.getAttribute("height")}`);
    //chart.setAttribute("style", "max-width: 100%; height: auto; height: intrinsic;");
}