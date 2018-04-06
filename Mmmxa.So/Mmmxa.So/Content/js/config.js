var ver = '3.1';
if (localStorage.version !== ver) {
    localStorage.removeItem('content');
    localStorage.version = ver;
}
