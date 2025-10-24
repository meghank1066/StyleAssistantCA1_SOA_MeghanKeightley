//reference: https://blog.logrocket.com/localstorage-javascript-complete-guide/

window.localStorageHelper = {
    // saves item and converts it to json before storing
    setItem: function (key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    //key to get stored item by key and parse it from JSON.
    getItem: function (key) {
        //item retrieval from localStorage 
        var item = localStorage.getItem(key);
        // if item exists, parse it, otherwise return empty array
        return item ? JSON.parse(item) : [];
    },
    // removes item from localStorage by key
    removeItem: function (key) {
        // removes item from localStorage
        localStorage.removeItem(key);
    }
};
