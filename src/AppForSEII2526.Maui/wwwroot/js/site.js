window.applyFontSize = (size) => {
    let root = document.documentElement;
    switch (size) {
        case "small":
            root.style.setProperty("--font-size-base", "12px");
            break;
        case "large":
            root.style.setProperty("--font-size-base", "18px");
            break;
        default:
            root.style.setProperty("--font-size-base", "14px");
            break;
    }
};