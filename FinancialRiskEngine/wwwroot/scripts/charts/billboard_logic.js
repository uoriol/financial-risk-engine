function CreateCashflowBarChart(years, in_array, out_array, divId) {
    const chart = bb.generate({
        data: {
            columns: [
                ["IN", ...in_array],
                ["OUT", ...out_array]
            ],
            type: "bar",
            colors: {
                IN: "#4CAF50",   // green
                OUT: "#F44336"   // red
            },
            groups: [["IN", "OUT"]]
        },
        bar: {
            width: {
                ratio: 0.6
            }
        },
        axis: {
            x: {
                type: "category",
                categories: years
            },
            y: {
                label: {
                    text: "Amount",
                    position: "outer-middle"
                }
            }
        },
        bindto: `#${divId}`
    });
}