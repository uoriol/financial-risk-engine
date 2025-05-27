
var efficientFrontierScatterplot;
function CreateScatterplot(data, divId, showLegend) {
    efficientFrontierScatterplot = bb.generate({
        data: _getFormattedScatterplotData(data),
        axis: {
            x: {
                label: "Higher.Volatility",
                tick: {
                    fit: false
                }
            },
            y: {
                label: "Higher.Return"
            }
        },
        legend: {
            show: showLegend
        },
        bindto: divId
    });
}

function CreateScatterplotWithTangent(data, tangentLineData, divId, showLegend) {
    efficientFrontierScatterplot = bb.generate({
        data: _getFormattedScatterplotDataWithTangent(data, tangentLineData),
        axis: {
            x: {
                label: "Higher.Volatility",
                tick: {
                    fit: false
                }
            },
            y: {
                label: "Higher.Return"
            }
        },
        point: {
            show: true
        },
        legend: {
            show: showLegend
        },
        bindto: divId
    });
}


function CreateScaterplotWithAxisMinZero(data, div, showLegend) {
    efficientFrontierScatterplot = bb.generate({
        data: _getFormattedScatterplotData(data),
        axis: {
            x: {
                label: "Higher.Volatility",
                tick: {
                    fit: false,
                    values: [0]
                },
                padding: {
                    left: 0
                },
                min: 0,
            },
            y: {
                label: "Higher.Return",
                tick: {
                    values: [0] // Ensure 0 is included in the y-axis ticks
                },
                padding: {
                    bottom: 0 // Remove bottom padding to ensure 0 is shown
                },
                min: 0,
            }
        },
        legend: {
            show: showLegend
        },
        bindto: divId
    });
}

function _getFormattedScatterplotData(data) {
    let xs_object = {};
    let columns_array = [];

    for (let i = 0; i < data.length; i++) {
        xs_object[data[i].name] = data[i].name + "_x";
        let item_array_x = [];
        let item_array_y = [];
        item_array_x.push(data[i].name + "_x");
        item_array_y.push(data[i].name);
        item_array_x.push(data[i].standardDeviation);
        item_array_y.push(data[i].expectedReturn);
        columns_array.push(item_array_x);
        columns_array.push(item_array_y);
    }

    return {
        xs: xs_object,
        columns: columns_array,
        type: "scatter"
    }
}

function _getFormattedScatterplotDataWithTangent(data, tangentLineData) {
    console.log("With tangent");
    let xs_object = {};
    let columns_array = [];
    let chart_types_object = {};

    for (let i = 0; i < data.length; i++) {
        xs_object[data[i].name] = data[i].name + "_x";
        let item_array_x = [];
        let item_array_y = [];
        item_array_x.push(data[i].name + "_x");
        item_array_y.push(data[i].name);
        chart_types_object[data[i].name] = "scatter";
        item_array_x.push(data[i].standardDeviation);
        item_array_y.push(data[i].expectedReturn);
        columns_array.push(item_array_x);
        columns_array.push(item_array_y);
    }
    xs_object["TangentLine"] = "TangentLine_x";
    chart_types_object["TangentLine"] = "line";
    console.log(tangentLineData);
    columns_array.push(["TangentLine_x", ...tangentLineData.map(d => d.x)]);
    columns_array.push(["TangentLine", ...tangentLineData.map(d => d.y)]);

    return {
        xs: xs_object,
        columns: columns_array,
        types: chart_types_object
    }
}