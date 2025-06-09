function representSimulation(priceData, field, divId, measureRiskValues = undefined) {
    //console.log(divId);
    //console.log(priceData);
    const chartConfig = {
        data: {
            x: "x",
            columns: [
                getDateAxis(priceData[0]),
                getPricePlotData(priceData[0], "data0", field)
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
        zoom: { enabled: true, type: "drag" },
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
        if (measureRiskValues != undefined && Array.isArray(measureRiskValues)) {
            const color = getSharpeColor(measureRiskValues[i]);

            setTimeout(function () {
                simulationchart.load({
                    columns: [getPricePlotData(priceData[i], seriesName, field)],
                    colors: {
                        [seriesName]: color
                    }
                });
            }, timePass);
        } else {
            setTimeout(function () {
                simulationchart.load({
                    columns: [getPricePlotData(priceData[i], seriesName, field)]
                });
            }, timePass);
        }
    }
}

function plotLineWithBand(priceData1, priceData2, divId, labelLine = "Main") {
    const x = ["x"];
    const mainLine = [labelLine];
    const lowerBand = ["lowerBand"];
    const bandGap = ["bandGap"];

    priceData1.forEach((d, i) => {
        const date = d.date;
        const base = d.close;
        const estimation = priceData2[i].close;

        const lower = priceData1[i].open - (priceData1[i].open * priceData2[i].return); // Add volatility to the lower band
        const upper = priceData1[i].open + (priceData1[i].open * priceData2[i].return); // Add volatility to the upper band

        x.push(date);
        mainLine.push(base);
        lowerBand.push(lower);
        bandGap.push(upper - lower);
    });

    bb.generate({
        bindto: "#" + divId,
        data: {
            x: "x",
            columns: [x, mainLine, lowerBand, bandGap],
            types: {
                [labelLine]: "scatter",
                lowerBand: "area",
                bandGap: "area"
            },
            groups: [
                ["lowerBand", "bandGap"]
            ],
            order: null
        },
        point: {
            show: true,
            r: function (d) {
                return d.id === labelLine ? 3 : 0;
            }
        },
        color: {
            pattern: ["#1f77b4", "rgba(173,216,230,0.2)", "rgba(173,216,230,0.9)"]
        },
        axis: {
            x: {
                type: "timeseries",
                tick: {
                    format: "%Y-%m-%d"
                }
            },
            y: {
                min: Math.min(...priceData1.map(d => d.close)),
                padding: {
                    top: 10,
                    bottom: 10
                }
            }
        },
        tooltip: {
            grouped: false
        },
        legend: {
            hide: ["lowerBand", "bandGap"]
        }
    });
}

function plotPointsAndLineoldd(priceData1, priceData2, divId, name1, name2) {

    const x = ["x"];
    const return1 = ["return1"];
    const return2 = ["return2"];

    priceData1.forEach((d, i) => {
        x.push(d.date);
        return1.push(d.return);
        return2.push(priceData2[i].return);
    });

    bb.generate({
        bindto: "#" + divId,
        data: {
            x: "x",
            columns: [
                x,
                return1,
                return2
            ],
            types: {
                return1: "scatter",  // We'll use "line", but disable the line via config
                return2: "line"
            }
        },
        point: {
            show: true,
            r: function (d) {
                // Show points only for return1
                return d.id === "return1" ? 4 : 0;
            }
        },
        line: {
            // Set line for return1 to invisible
            classes: {
                return1: "no-line"
            }
        },
        axis: {
            x: {
                type: "timeseries",
                tick: {
                    format: "%Y-%m-%d",
                    count: 10
                }
            }
        }
    });
}

function plotPointsAndLine(priceData1, priceData2, divId, name1, name2) {

    const x = ["x"];
    const return1 = [name1];
    const return2 = [name2];

    priceData1.forEach((d, i) => {
        x.push(d.date);
        return1.push(d.return);
        return2.push(priceData2[i].return);
    });

    bb.generate({
        bindto: "#" + divId,
        data: {
            x: "x",
            columns: [
                x,
                return1,
                return2
            ],
            types: {
                [name1]: "scatter",
                [name2]: "line"
            }
        },
        point: {
            show: true,
            r: function (d) {
                // Show points only for name1
                return d.id === name1 ? 4 : 0;
            }
        },
        line: {
            classes: {
                [name1]: "no-line"
            }
        },
        axis: {
            x: {
                type: "timeseries",
                tick: {
                    format: "%Y-%m-%d"
                }
            }
        }
    });
}

function getDateAxis(prices) {
    let xAxisArray = ["x"];
    for (let datePoint = 0; datePoint < prices.length; datePoint++) {
        xAxisArray.push(formatDate(prices[datePoint].date));
    }

    return xAxisArray;
}

function getPricePlotData(prices, name, field = "close") {
    let yAxisPrice = [name];
    for (let p = 0; p < prices.length; p++) {
        yAxisPrice.push(prices[p][field]);
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