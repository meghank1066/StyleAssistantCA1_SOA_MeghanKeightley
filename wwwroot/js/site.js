//window.saveToLookbook = function (item) {
//    let lookbook = JSON.parse(localStorage.getItem("lookbook")) || [];
//    // prevent duplicates
//    if (!lookbook.some(i => i.Name === item.Name)) {
//        lookbook.push(item);
//        localStorage.setItem("lookbook", JSON.stringify(lookbook));
//        alert(`Added ${item.Name} to your lookbook 💅`);
//    } else {
//        alert(`${item.Name} is already in your lookbook!`);
//    }
//};

window.localStorageHelper = {
    getItem: (key) => {
        return JSON.parse(localStorage.getItem(key) || '[]');
    },
    setItem: (key, value) => {
        localStorage.setItem(key, JSON.stringify(value));
    }
};
window.saveToLookbook = (item) => {
    let lookbook = JSON.parse(localStorage.getItem('lookbook') || '[]');
    if (!lookbook.some(i => i.ProductUrl === item.ProductUrl)) {
        lookbook.push(item);
        localStorage.setItem('lookbook', JSON.stringify(lookbook));
    }
};

