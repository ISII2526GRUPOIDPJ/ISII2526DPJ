window.applyDarkMode = {
    setDarkMode: function (isDark) {
        if (isDark) {
            document.body.classList.add('dark-mode');
        } else {
            document.body.classList.remove('dark-mode');
        }
        return true;
    }
};