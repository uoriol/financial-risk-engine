async function CreateVaRHoldingPeriodAndCL3DChart(data, selector) {
    console.log("Rendering VaR 3D Surface");

    // Load Plotly dynamically if not already loaded
    if (typeof Plotly === 'undefined') {
        await loadPlotly();
    }

    // Extract unique X and Y axis values
    const confidenceLevels = [...new Set(data.map(d => d.confidenceLevel))].sort((a, b) => a - b);
    const holdingPeriods = [...new Set(data.map(d => d.holdingPeriod))].sort((a, b) => a - b);

    // Build Z matrix: rows = holdingPeriods, columns = confidenceLevels
    const z = holdingPeriods.map(hp => {
        return confidenceLevels.map(cl => {
            const item = data.find(d => d.holdingPeriod === hp && d.confidenceLevel === cl);
            return item ? item.vaR : null;
        });
    });

    const plotData = [{
        type: 'surface',
        x: confidenceLevels,
        y: holdingPeriods,
        z: z,
        colorscale: 'Viridis',
        contours: {
            z: {
                show: true,
                usecolormap: true,
                highlightcolor: "#42f462",
                project: { z: true }
            }
        }
    }];

    const layout = {
        title: 'VaR Surface: Holding Period vs Confidence Level',
        autosize: true,
        scene: {
            xaxis: {
                title: { text: 'Confidence Level' },
                tickformat: '.2f'
            },
            yaxis: {
                title: { text: 'Holding Period' },
                tickformat: 'd'
            },
            zaxis: {
                title: { text: 'VaR' },
                tickformat: '.2f'
            }
        }
    };

    // Remove <svg> and insert Plotly chart into its parent
    const container = document.querySelector(selector);
    if (!container) {
        console.error("Container not found:", selector);
        return;
    }

    // Optional: replace SVG placeholder with a div
    const parent = container.parentElement;
    parent.innerHTML = `<div id="var3dplot" style="height: 500px;"></div>`;
    Plotly.newPlot('var3dplot', plotData, layout);


}

function loadPlotly() {
    return new Promise((resolve, reject) => {
        const script = document.createElement("script");
        script.src = "https://cdn.plot.ly/plotly-3.0.1.min.js";
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}