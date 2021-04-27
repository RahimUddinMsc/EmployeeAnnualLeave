var popupComponent = (function () {

    let DOMStrings = {
        parentContainer: ".body-panel-right",
        promptClass: "popup-component-container",
        popupClose: ".popup-component-close",
        slideAnimation: "pg-slide-right"
    }

    let template = `
        <div class = '${DOMStrings.promptClass} content-panel-bg'>
             <div class = "popup-component-close">
                X
             </div>
        </div>`;
    
    let setupEventListener = function () {
        let el = document.querySelector(DOMStrings.popupClose)

        el.parentElement.addEventListener('transitionend', e => {
            removePrompt();
        });

        el.addEventListener('click', e => {
            popupClosure();
        });

    }

    let popupClosure = function () {
        let el = document.querySelector(DOMStrings.popupClose)
        document.querySelector('.' + DOMStrings.promptClass).classList.add('popup-component-closure');
        document.querySelector(DOMStrings.parentContainer).classList.remove(DOMStrings.slideAnimation);
        el.parentElement.removeChild(el);
    }


    //removes any prompts that may be active 
    let removePrompt = function () {
        let popupDom = document.querySelector('.' + DOMStrings.promptClass);
        if (popupDom) {
            popupDom.parentElement.removeChild(popupDom);
            document.querySelector(DOMStrings.parentContainer).classList.remove(DOMStrings.slideAnimation);
        }        
    }

    let generatePrompt = function (el, evObj) {

        removePrompt();

        //handle the close button
        document.querySelector('body').insertAdjacentHTML('beforeend', template);       
        document.querySelector(DOMStrings.parentContainer).classList.add(DOMStrings.slideAnimation);
        setupEventListener();

        //Add element passed and assign any event listener to buttons passed 
        document.querySelector('.' + DOMStrings.promptClass).insertAdjacentHTML('beforeend', el);
        for (let i = 0; i < evObj.length; i++) {
            obj = evObj[i];

            document.querySelector('.' + DOMStrings.promptClass + " " + obj.el).addEventListener('click', e => {
                evObj[i].event();
                removePrompt();
            })
        }

    }

    return {

        popup: function (el, evObj) {
            generatePrompt(el, evObj);
        },

        close: function () {
            removePrompt()
        }

    }


})()