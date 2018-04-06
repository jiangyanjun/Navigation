var ver = '1.0.0.0';
if (localStorage.version !== ver) {
    localStorage.removeItem('content');
    localStorage.version = ver;
}
