function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}
$.validator.methods.date = function (value, element) {
    return this.optional(element) || parseDate(value) !== null;
}