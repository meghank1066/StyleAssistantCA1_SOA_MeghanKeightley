//reference https://www.w3schools.com/html/html5_webstorage.asp?
//localStorage allows web applications to store data persistently in the browser.

// Define a helper object to make working with localStorage easier
window.localStorageHelper = {
    // get stored item by key and parse it from JSON.
    // If the key doesn't exist, it returns an empty array by default.
    getItem: (key) => {
        return JSON.parse(localStorage.getItem(key) || '[]');
    },

    //saves item and converts it to json before sorting
    setItem: (key, value) => {
        localStorage.setItem(key, JSON.stringify(value));
    }
};

// Function to save an item to the lookbook in localStorage
window.saveToLookbook = (item) => {
    // Get current lookbook items from localStorage, or start with an empty array
    let lookbook = JSON.parse(localStorage.getItem('lookbook') || '[]');

    // Check if the item doesn't exist
    if (!lookbook.some(i => i.Name === item.Name && i.Brand === item.Brand)) {
        lookbook.push(item); // Add the item to the lookbook

        // Save the updated lookbook back to localStorage
        localStorage.setItem('lookbook', JSON.stringify(lookbook)); // savesthe new looks to our lookbook with local storage
    } else {
        console.log(`${item.Name} is already in your lookbook.`);
    }
};


