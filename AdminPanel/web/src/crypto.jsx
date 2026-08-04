function encrpty (data) {
    return btoa(data);
}

function decrpty (data) {
    return atob(data);
}

export { encrpty, decrpty };