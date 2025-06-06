function representSimulation(priceData, divId, measureRiskValues = undefined) {
    console.log(divId);
    console.log(priceData);
    const chartConfig = {
        data: {
            x: "x",
            columns: [
                getDateAxis(priceData[0]),
                getPricePlotData(priceData[0], "data0")
            ],
            type: "line"
        },
        axis: {
            x: {
                type: "timeseries",
                tick: {
                    format: "%Y-%m-%d",
                    count: 10
                }
            }
        },
        legend: { show: false },
        tooltip: { show: false },
        point: { show: false },
        zoom: { enabled: false, type: "drag" },
        bindto: "#" + divId
    };

    // Only apply custom colors if measureRiskValues is not null/undefined
    if (measureRiskValues != undefined && Array.isArray(measureRiskValues)) {
        chartConfig.data.colors = getLineColors(priceData, measureRiskValues);
    }

    const simulationchart = bb.generate(chartConfig);

    for (let i = 1; i < priceData.length; i++) {
        let timePass = (100 * i) + 100;

        const seriesName = "data" + i;
        const color = getSharpeColor(measureRiskValues[i]);

        setTimeout(function () {
            simulationchart.load({
                columns: [getPricePlotData(priceData[i], seriesName)],
                colors: {
                    [seriesName]: color
                }
            });
        }, timePass);
    }
}


function getDateAxis(prices) {
    let xAxisArray = ["x"];
    for (let datePoint = 0; datePoint < prices.length; datePoint++) {
        xAxisArray.push(formatDate(prices[datePoint].date));
    }

    return xAxisArray;
}

function getPricePlotData(prices, name) {
    let yAxisPrice = [name];
    for (let p = 0; p < prices.length; p++) {
        yAxisPrice.push(prices[p].close);
    }

    return yAxisPrice;
}

function getLineColors(prices, riskValues) {

    // First, build the colors object dynamically based on measureRiskValues
    const lineColors = {};
    for (let i = 0; i < prices.length; i++) {
        const lineName = "data" + i;
        lineColors[lineName] = getSharpeColor(riskValues[i]);
    }

    return lineColors;
}

function getSharpeColor(value) {
    if (value > 2) {
        return "green";
    }
    if (value > 1) {
        return "#9fb596";
    }
    if (value > 0.5) {
        return "#7ea4c2";
    }
    if (value > 0) {
        return "#e65e5e";
    }
    return "red";
}


function formatDate(date) {
    if (!(date instanceof Date)) {
        date = new Date(date); // Convert to Date object if it's not already
    }
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}