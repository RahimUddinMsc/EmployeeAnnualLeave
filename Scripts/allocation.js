var dataController = (function () {

    class CalDateSelected {
        constructor(month, year) {
            this.month = month;
            this.year = year;
        }
    }

    class AllocationRequest {
        constructor(calendarId, timeSet) {
            this.calendarId = calendarId;
            this.timeSet = timeSet;
        }        
    }

    //js tracker
    let allocationList = []

    //api call
    let allocationReqList = []  

    return {

        setSelectedDate: function (month, year) {
            this.calDate = new CalDateSelected(month, year);
        },

        getCalDateSelected: function () {
            return this.calDate;
        },

        updateCalDate: function (month, year) {
            this.calDate.month = month;
            this.calDate.year = year;
        },

        getAllocationList: function () {
            return allocationList
        },

        clearAllocationList: function () {
            return allocationList = []
        },

        checkAllocationReq: function (calID) {
            return allocationList.includes(calID)
        },

        addAllocationReq: function (calID) {
            allocationList.push(calID)
        },

        removeAllocationReq: function (calID) {            
            allocationList.splice(allocationList.indexOf(calID),1)
            
        },

        addAllocationReqObj: function (calID, mins) {            
            allocationReqList.push(new AllocationRequest(calID, mins))
        },

        clearAllocationReqList: function () {
            allocationReqList = []
        },

        getAllocationApiList: function () {
            return allocationReqList;
        }


    }


})()

var UIController = (function () {


    let DOMStrings = {
        curDateMonth: ".curr-month-date",
        curDateYear: ".curr-year-date",   
        curDatePrompt: ".cur-date-block",
        allocationMinValue: ".allocation-min-value",
        calBlock:".cal-date-block",
        calendarContainer: ".calendar-container",
        dateInputMonth: "#date-input-month",
        dateInputYear: "#date-input-year",
        dateChangeSubmit: "date-change-submit",
        dateChangeBtn: ".btn-month-change",
        prevMonthBtn: "prev-month-btn",
        nextMonthBtn: "next-month-btn",
        timeAllocationInput: "time-allocation-input",
        allocationInputBtns: ".allocation-input-btns",
        allocationIncrease: "allocation-btn-increase",
        allocationSet: "allocation-btn-set",
        allocationDecrease: "allocation-btn-decrease",
        submitAllocation: "submit-allocation"
    }

    let setupDateSelect = function (data) {
        let months = ["n/a", "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
        document.querySelector(DOMStrings.curDateMonth).innerText = months[data.month]
        document.querySelector(DOMStrings.curDateYear).innerText = data.year
    }

    let alignCalendarDays = function (month,year) {


        let calBlock = document.querySelector(DOMStrings.calendarContainer)
        let dte = new Date(year, month - 1, 1)

        //create date for 1st day of month and check which weekday it falls on
        let weekday = new Date(year, month - 1, 1).getDay()
        if (weekday === 0) {weekday = 7}
        
        //Now fill in entry block for the number of days weekdays which would fall at the end of the month
        for (i = (weekday - 1); i > 0; i--) {
            calBlock.insertAdjacentHTML("afterbegin", '<div class = "cal-block-disabled"></div>')
        }       

        //Get the last day of the month and find out which weekday it falls on and if it's not on a sunday add an empty block for each weekday until it is sat (5) + 1 to go to sun
        let nwDate = new Date(dte.setMonth(dte.getMonth() + 1))
        let lastDay = new Date (nwDate.setDate(nwDate.getDate() - 1)).getDay()

        if (lastDay !== 0) {
            for (x = lastDay; x <= 6; x++) {
                calBlock.insertAdjacentHTML("beforeend", '<div class = "cal-block-disabled"></div>')
            }
        }
              
    }

    let dateChangeHTML = function () {
        
        let htmlString =
            `
            <div class = 'prompt-cal-container'>
                <div class="prompt-title-header">
                    <h2>Date Select</h2>
                </div>
                <div class = "date-change-block">
                    <div class = "month-entry-block">
                        <h3>Month</h3>
                        <input type = "text" id = "date-input-month"  />
                    </div>
                    <div class = "date-entry-divider">:</div>
                    <div class = "year-entry-block">
                        <h3>Year</h3>
                        <input type = "text" id = "date-input-year"  />
                    </div>
                </div>
                <div class = "date-change-submit prompt-action-container">                    
                    <span>Submit</span>
                </div>
            </div>
            `
        return htmlString

    }


    function updateAllocationTimes(arr,type) {
        let mins = parseInt(document.getElementById(DOMStrings.timeAllocationInput).value);       
        for (i = 0; i < arr.length; i++) {
            let domEl = document.querySelector(`[data-cal-id*="${arr[i]}"]`);
            let el = domEl.querySelector(DOMStrings.allocationMinValue);
            if (type === "increase") {
                el.dataset.allocatedMins = parseInt(el.dataset.allocatedMins) + mins;                
            } else if (type === "decrease") {
                el.dataset.allocatedMins = parseInt(el.dataset.allocatedMins) - mins;
            } else {
                el.dataset.allocatedMins = parseInt(mins);
            }            
            el.innerText = calcHours(el.dataset.allocatedMins);
        }
    }


    let calcHours = function(mins) {
        let hours = Math.floor(mins / 60)
        let minutes = mins % 60
        return hours + "h " + minutes + "m"
    }
   
    return {

        getDomStrings: function () {
            return DOMStrings;
        },

        setupDateHeading: function (data) {
            //updates month and year span tags with date
            setupDateSelect(data)
        },

        alignDays: function (month, year) {
            alignCalendarDays(month,year)
        },

        setupDateChangeRequest: function (data, el) {
            return dateChangeHTML()
        },

        setupCellForAllocationReq: function (data) {
            let domEl = document.querySelector(`[data-cal-id*="${data}"]`)            
            domEl.style.backgroundColor === "rgba(210, 187, 64, 0.68)" ? domEl.style.backgroundColor = "#1b4e5dcf" : domEl.style.backgroundColor = "#d2bb40ad"
        },

        updateMinsSet: function (arr,type) {
            updateAllocationTimes(arr,type)
        }

    }


})()

var controller = (function (dataCtrl, UICtrl, apiCtrl) {

    let domStrings = UIController.getDomStrings()

    document.querySelectorAll(domStrings.dateChangeBtn).forEach(e => {
        e.addEventListener('click', ev => {
            e.classList.contains(domStrings.prevMonthBtn) ? updateDateSelection(false) : updateDateSelection(true)
        });
    });
    document.querySelector(domStrings.curDatePrompt).addEventListener("click", e => setupDateSelectPrompt());
    document.querySelector(domStrings.allocationInputBtns).addEventListener("click", function (e) {
        let type
        if (e.target.classList.contains(domStrings.allocationIncrease)) {
            type = "increase"
        } else if (e.target.classList.contains(domStrings.allocationDecrease)) {
            type = "decrease"
        } else {        
            type = "set"
        }
       
        UICtrl.updateMinsSet(dataCtrl.getAllocationList(), type);
      
    })
    document.getElementById(domStrings.submitAllocation).addEventListener("click", sendAllocationRequest)

   
    function setupEventListeners() {
        document.querySelectorAll(domStrings.calBlock).forEach(e => {
            e.addEventListener('click', ev => {
                setupAllocationRequest(e)   
            });
        }); 
    }

    function sendAllocationRequest() {        
        let nodeLi = document.querySelectorAll("[data-cal-id]")
        dataCtrl.clearAllocationReqList()
        
        for (i = 0; i < nodeLi.length; i++) {            
            dataCtrl.addAllocationReqObj(nodeLi[i].dataset.calId, nodeLi[i].querySelector(domStrings.allocationMinValue).dataset.allocatedMins)            
        }

        console.log(dataCtrl.getAllocationApiList())
        apiCtrl.UpdateAllocationDays(dataCtrl.getAllocationApiList()).then(e => {
            //clear any highlighted cells
            let calList = dataCtrl.getAllocationList()
            for (x = 0; x < calList.length; x++) {  
                UICtrl.setupCellForAllocationReq(calList[x]);        
            }
            dataCtrl.clearAllocationList();
        }).catch(function (err) {
            alert(err)
        });
    }

    function setupAllocationRequest(element) {
        // -1 means does not exist
        let calID = element.dataset.calId;        
        
        if (dataCtrl.checkAllocationReq(calID)) {
            ;
        } else {
            dataCtrl.addAllocationReq(calID);            
        }
        
        UICtrl.setupCellForAllocationReq(calID)        
    }

    function updateDateSelection(next) {

        //get the current date selected
        let dteChosen = dataCtrl.getCalDateSelected();
        let yr = dteChosen.year;
        let month = dteChosen.month;

        //if true going forward one month
        next ? month += 1 : month -= 1

        //if going forward a month and and is on 13 them month needs to be changed to 1 and the year incremented
        if (month === 13) {
            month = 1
            yr += 1
        }

        //if going back a month and month is 0 them month needs to be changed to 12 and the year decrement
        if (month === 0) {
            month = 12
            yr -= 1
        }

        //call data update the intialize to new dates 
        updateAllocationDaysDom(month,yr); 
    }

    function setupDateSelectPrompt() {
        let promptEl = UICtrl.setupDateChangeRequest()
        popupComponent.popup(promptEl, [
            {
                el: "." + domStrings.dateChangeSubmit,
                event: function () {                    
                    updateAllocationDaysDom(parseInt(document.querySelector(domStrings.dateInputMonth).value), parseInt(document.querySelector(domStrings.dateInputYear).value));
                }
            },
        ]);
    }

    function updateAllocationDaysDom(month, yr) {
        apiCtrl.getAllocationDaysForMonth(month, yr).then(function (data) {
            dataCtrl.updateCalDate(month, yr);
            document.querySelector(domStrings.calendarContainer).parentElement.innerHTML = data;
            init();
        }).catch(function (err) {
            alert(err);
        });

    }

    function init() {
        let dteChosen = dataCtrl.getCalDateSelected();       
        UICtrl.setupDateHeading(dteChosen);
        UICtrl.alignDays(dteChosen.month, dteChosen.year)
        setupEventListeners()
    }

    return {

        initialize: function (month, year) {
            dataCtrl.setSelectedDate(month, year);
            init();
        }

    }


})(dataController, UIController, apiController)

var dte = new Date(Date.now())
controller.initialize(dte.getMonth() + 1 ,dte.getFullYear());
