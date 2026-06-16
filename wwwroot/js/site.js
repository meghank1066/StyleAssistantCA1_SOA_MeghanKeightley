//// reference: https://www.w3schools.com/html/html5_webstorage.asp
//// localStorage allows web applications to store data persistently in the browser.

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


//changed function as it only added first item saved not lots
window.saveToLookbook = (item) => {
    const formattedItem = {
        Name: item.name || item.Name,
        Brand: item.brand || item.Brand,
        Brand: item.brand || item.Brand,
        Price: item.price || item.Price,
        ImageUrl: item.imageUrl || item.ImageUrl,
        ProductUrl: item.productUrl || item.ProductUrl,
        Colour: item.colour || item.Colour
    };
    let lookbook = window.localStorageHelper.getItem('lookbook');
    if (!lookbook.some(i => i.Name === formattedItem.Name && i.Brand === formattedItem.Brand)) {
        lookbook.push(formattedItem);
        window.localStorageHelper.setItem('lookbook', lookbook);
        console.log(`${formattedItem.Name} added to your lookbook!`);
    } else {
        console.log(`${formattedItem.Name} is already in your lookbook.`);
    }
};

//// Optional: helper to clear the lookbook (for testing)
window.clearLookbook = () => {
    localStorage.removeItem('lookbook');
    console.log('Lookbook cleared!');
};
