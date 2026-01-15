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