window.storeDotNetRef = (dotNetHelper) => {
    window.blazorDotNetRef = dotNetHelper;
};

window.initMermaidWithBlazorRouting = (dotNetHelper) => {
    // Function is now just for initialization if needed
    console.log('Mermaid routing initialized');
};

window.blazorNavigateToRoute = (route) => {
    if (window.blazorDotNetRef) {
        window.blazorDotNetRef.invokeMethodAsync('NavigateToRoute', route);
    }
};

window.displayLogMessages = (stepId) => {
    if (window.blazorDotNetRef) {
        window.blazorDotNetRef.invokeMethodAsync('DisplayLogMessages', stepId);
    }
};

window.refreshMermaidDiagram = async () => {
    try {
        // Just remove the processed markers - Blazor has recreated the element
        document.querySelectorAll('.mermaid').forEach(el => {
            el.removeAttribute('data-processed');
        });

        await mermaid.run();
    } catch (error) {
        console.error('Mermaid refresh error:', error);
    }
};

window.initializeMermaid = () => {
    mermaid.initialize({
        startOnLoad: false,
        securityLevel: 'loose',
        theme: 'base',
        themeVariables: {
            fontFamily: 'JetBrains Mono, sans-serif',
            primaryColor: '#1a4d2e',
            primaryTextColor: '#3a881b',
            primaryBorderColor: '#3a881b',
            lineColor: '#3a881b',
            secondaryColor: '#2d6a4f',
            tertiaryColor: '#081c15',
            background: '#081c15',
            mainBkg: '#e8f9eb',
            secondBkg: '#2d6a4f',
            tertiaryBkg: '#52b788',
            nodeBorder: '#74c69d',
            clusterBkg: '#1a4d2e',
            clusterBorder: '#2d6a4f',
            textColor: '#3a881b',
            edgeLabelBackground: '#9dcc8e'
        }
    });
};