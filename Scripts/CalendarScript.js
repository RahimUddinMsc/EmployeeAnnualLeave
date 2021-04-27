
//data controller will hold all of the objects needed to track the calendar data
var dataController = (function () {

    class UserInfo {
        constructor(minutesAvailable, minutesUsed,fullDayAllocation) {
            this.minutesAvailable = minutesAvailable;
            this.minutesUsed = minutesUsed;
            this.fullDayAllocation = fullDayAllocation;
        }
    }


    class CalDateSelected{
        constructor(month, year){
            this.month = month;
            this.year = year;
        }
    }

    class DBHoliday {
        constructor(calendarId, bookId, empLinkId, approvalId, numQueue,pending,customTime,startTime,endTime) {
            this.CalendarId = calendarId;
            this.BookId = bookId;
            this.EmpLinkId = empLinkId;
            this.ApprovalId = approvalId;
            this.NumQueue = numQueue;
            this.Reason = pending.reason;
            this.Response = pending.response;
            this.CustomTime = customTime;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }

    class DBHolidayList {
        constructor(dBHolList) {
            this.DBHolList = dBHolList;            
        }
    }

    class HolidayRequest {
        constructor(calendarId,minutesRequested,customTime,start,end) {
            this.CalendarId = calendarId;
            this.MinutesRequested = minutesRequested;
            this.CustomTime = customTime;
            this.Start = start;
            this.End = end;
        }        
    }

    class HolidayRequestList {
        constructor(requestList) {
            this.RequestList = requestList;            
        }
    }

    class CalDays {
        constructor(calID, day, month, yr, availableMins) {
            this.calID = calID;
            this.day = day;
            this.month = month;
            this.yr = yr;
            this.availableMins = availableMins;
            this.calcHours();
            this.checkWkend();
        }

        //converts mins to hours + mins 
        calcHours() {
            let hours = Math.floor(this.availableMins / 60)
            let mins = this.availableMins % 60
            this.timeRem = hours + "h " + mins + "m"
        }

        //checks for weekend so can grey out in cal
        checkWkend() {
            let dte = new Date(this.yr, this.month - 1, this.day)
            dte.getDay() === 0 || dte.getDay() === 6 ? this.isWeekend = true : this.isWeekend = false;
        }
    }

    let calendarList = [];
    let holidays = new HolidayRequestList([]);
       
    let checkHolidayRequestExists = function(id){
        for (i = 0; i < holidays.RequestList.length; i++) {
            if (holidays.RequestList[i].CalendarId === id) {                
                return i;
            }
        } 
        return -1;
    }

    let dbHolList = []

    let getHolFromDBHolList = function (calID) {
        for (i = 0; i < dbHolList.length; i++) {
            if (dbHolList[i].CalendarId == calID) {
                return dbHolList[i];
            }
        } 
    }

    return {

        setUserInfoObj: function (data) {
            this.userInfo = new UserInfo(data.minutesAvailable, data.minutesUsed, data.fullDayAllocation);
        },

        updateUserInfoObj: function (mins,booked) {
            if (booked) {
                this.userInfo.minutesAvailable -= mins;
                this.userInfo.minutesUsed += mins;
            } else {
                this.userInfo.minutesAvailable += mins;
                this.userInfo.minutesUsed -= mins;
            }
        },

        getUserInfoObj: function () {
            return this.userInfo;
        },

        setSelectedDate: function (month, year) {
            this.calDate = new CalDateSelected(month, year);
        },

        updateCalDate: function (month, year) {
            this.calDate.month = month;
            this.calDate.year = year;
        },

        getCalDateSelected: function () {
            return this.calDate;
        },

        //Holiday request functions
        setCal: function (data) {
            for (i = 0; i < data.length; i++) {
                calendarList.push(new CalDays(data[i].calendarId, data[i].day, data[i].month, data[i].year, data[i].availableMinutes))
            }            
        },

        getCalendarData: function() {
            return calendarList;
        },

        clearCalendarData: function () {
            calendarList = [];
        },

        CheckForHoliday: function (id) {
            return checkHolidayRequestExists(id);
        },

        addHolidayRequest: function (id, day, month, year, mins) {            
            holidays.RequestList.push(new HolidayRequest(id, day, month, year, mins));         
        },
        
        removeHolidayRequest: function (id) {
            holidays.RequestList.splice(id, 1);  
        },

        getHolidayRequests: function () {
            return holidays;
        },

        clearHolidayRequests: function () {
            holidays.RequestList = [];
        }, 

        //User db holiday functions
        setDBHol: function (data) {
            for (i = 0; i < data.length; i++) {
                dbHolList.push(new DBHoliday(data[i].calendarId, data[i].bookId, data[i].empLinkId, data[i].approval.approvalId, data[i].numQueue, data[i].pending, data[i].customTimeSet, data[i].customTime.startTime, data[i].customTime.endTime));
            }
        },

        getDBHols: function () {
            return dbHolList;
        },

        getDBHolFromList: function (calID) {
            return getHolFromDBHolList(calID);
        },

        clearDBHols: function (data) {
            dbHolList = [];
        },

        clearAllData: function (data) {
            calendarList = [];
            holidays.RequestList = [];
            dbHolList = [];
        },
       
    }


})();

//ui controller will work on the display aspects of the code 
var UIController = (function() {

    let DOMStrings = {
        calDteHeading:".cal-dte-heading",
        curDateMonth: ".curr-month-date",
        curDateYear: ".curr-year-date",
        curDatePrompt: ".cur-date-block",
        dateInputMonth: "#date-input-month",
        dateInputYear: "#date-input-year",
        dateChangeSubmit:"date-change-submit",
        prevMonthBtn: "prev-month-btn",
        nextMonthBtn: "next-month-btn",
        holAvailableVal: "hol-available-val",
        holUsedVal: "hol-used-val",
        dateChangeBtn: ".btn-month-change",
        calendarContainer: ".calendar-container",
        calBlockActive: ".cal-block-active",
        calBlockApproved: ".cal-block-status-1",
        calBlockPending: ".cal-block-status-3",
        calBlockQueue: ".cal-block-status-4",
        calBlockInsufficient: ".cal-block-status-5",
        calBlockDate: ".cal-block-date",        
        calBlockTime: ".cal-block-time",        
        promptCancel: "prompt-action-cancel",
        promptOverideBtn: "prompt-action-overide",
        promptCustomHolSubmitBtn: "prompt-action-custom-hol",
        promptOverideInput: "prompt-overide-input",
        bookConfirm: "#bookConfirm",
        customStartTime: "startTime",
        customEndTime: "endTime",
    }


    let setupDateSelect = function (data) {
        let months = ["n/a", "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
        document.querySelector(DOMStrings.curDateMonth).innerText = months[data.month]
        document.querySelector(DOMStrings.curDateYear).innerText = data.year
    }

    let dayTemplate = function(data) {
        let htmlString = ""      
        let today = new Date()        

        // look through cal items and populate html
        for (i = 0; i < data.length; i++) {            

            //The days need to line up with the date e.g 3 -> Wed
            //As a result find which weekday first cal obj data lands on and fill with blanks
            if (i === 0) {
                let dte = new Date(data[i].yr, data[i].month-1, data[i].day).getDay()
                if (dte === 0) {dte = 7}
                for (y = dte - 1; y > 0; y--) {
                    htmlString += `<div class = 'cal-block-invalid'></div>`
                }
            }

            //setup class will only be an active date if is not on a weekend or in the past
            let curDte = new Date(data[i].yr, data[i].month - 1, data[i].day)
            let calClass;
            if (data[i].isWeekend || curDte < today) {
                calClass = "cal-block-disabled"
            } else {
                calClass = "cal-block-active"
            }
            //data[i].isWeekend ? calClass = "cal-block-disabled" : calClass = "cal-block-active";

            htmlString += `
            <div class = '${calClass + " content-panel-bg"}' data-cal-id = ${data[i].calID} >
                <div class = cal-block-date><span>${data[i].day}</span></div>
                <div class = cal-block-time><span>${data[i].timeRem}</span></div>
            </div>`;

            //check what the last day falls on and fill rest of the grid with blank slots
            if (i === data.length - 1) {
                let lasdte = new Date(data[i].yr, data[i].month - 1, data[i].day).getDay()
                if (lasdte !== 0) {
                    for (z = lasdte; z < 7; z++) {
                        htmlString += `<div class = 'cal-block-invalid'></div>`
                    }
                }
            }
        }

        return htmlString
    }

    let holidayCancellationHTML = function (data, el) {

        //Grab tje 
        //console.log(el.innerHTML)
        let calDay = el.querySelector(DOMStrings.calBlockDate).innerText;
        let bg = window.getComputedStyle(el, null).backgroundColor

        let htmlString = 
        `
            <div class = 'prompt-cal-container'>
                <div class="prompt-title-header">
                    <h2>Holiday Cancellation</h2>
                </div>
                <div class='prompt-container-cancellation'>
                    <div class='prompt-date-block'><span>${calDay}<span class='dte-suffix-val'>${getDteSuffix(parseInt(calDay))}</span> </span></div>
                </div>
                <div class='prompt-action-container'}>
                    <div class=${DOMStrings.promptCancel}><span>Cancel</span></div>
                </div>                
            </div>
        `
        return htmlString

    }

    let requestOverideHTML = function (data, el) {

        //Grab tje 
        //console.log(el.innerHTML)
        let calDay = el.querySelector(DOMStrings.calBlockDate).innerText;        
        let bg = window.getComputedStyle(el, null).backgroundColor

        let htmlString =
            `
            <div class = 'prompt-cal-container'>
                <div class="prompt-title-header">
                    <h2>Overide Request</h2>
                </div>
                <div class='prompt-date-overide'>
                    <span class='popup-large-dte'>${calDay}<span class='dte-suffix-val'>${getDteSuffix(calDay)}</span></span>
                    <div class = prompt-queue-num>Queue : ${data.NumQueue}</div>
                </div>
                <div class='prompt-date-container'>                    
                    <div class= 'prompt-single-inputs-container'>  
                        <textarea class= 'prompt-single-div' id = ${DOMStrings.promptOverideInput} ></textarea>
                    </div>
                </div>
                <div class='prompt-action-container'}>
                    <div class=${DOMStrings.promptOverideBtn}>
                        <span>Send Request</span>
                    </div>
                    <div class=${DOMStrings.promptCancel}>
                        <span>Cancel</span>
                    </div>
                </div>                
            </div>
        `
        return htmlString

    }


    let requestPendingHTML = function (data, el) {

        //Grab tje 
        //console.log(el.innerHTML)
        let calDay = el.querySelector(DOMStrings.calBlockDate).innerText;
        let bg = window.getComputedStyle(el, null).backgroundColor

        let htmlString =
            `
            <div class = 'prompt-cal-container'>
                <div class="prompt-title-header">
                    <h2>Overide Request</h2>
                </div>
                <div class='prompt-custom-response'>
                    <div class='custom-response-date'>
                        <span class='popup-large-dte'>${calDay}<span class='dte-suffix-val'>${getDteSuffix(calDay)}</span></span>
                        <div class = prompt-queue-num>Queue : ${data.NumQueue}</div>
                    </div>
                    <div class = 'prompt-dual-inputs-container'>
                        <div class= 'prompt-dual-div'>  
                            <textarea class='prompt-single-div' id = ${DOMStrings.promptOverideInput} > Request: ${data.Reason}</textarea>
                        </div>
                        <div class= 'prompt-dual-div'>  
                            <textarea class='prompt-single-div' id = ${DOMStrings.promptOverideInput} > Response: ${data.Response}</textarea>
                        </div>  
                    </div>
                </div>
                <div class='prompt-action-container'>                  
                    <div class=${DOMStrings.promptCancel}>
                        <span>Cancel</span>
                    </div>
                </div>                
            </div>
        `
        return htmlString

    }

    let requestCustomHolHTML = function (el) {

        //Grab tje 
        //console.log(el.innerHTML)
        let calDay = el.querySelector(DOMStrings.calBlockDate).innerText;
        let bg = window.getComputedStyle(el, null).backgroundColor

        let htmlString =
            `
            <div class = 'prompt-cal-container'>
                <div class="prompt-title-header">
                    <h2>Custom Holiday</h2>
                </div>
                <div class='prompt-custom-hol-container'>
                    <div  class='prompt-date-overide'>
                        <span class='popup-large-dte'>${calDay}<span class='dte-suffix-val'>${getDteSuffix(calDay)}</span></span>                        
                    </div>
                    <div class = 'custom-time-select'>
                        <div class= ''>  
                            <label> Start Time</label>
                            <input type="time" id="${DOMStrings.customStartTime}">
                        </div>
                        <div class= ''>              
                            <label>End Time</label>
                            <input type="time" id="${DOMStrings.customEndTime}">
                        </div>  
                    </div>
                </div>
                <div class='prompt-action-container'}>                  
                    <div class=${DOMStrings.promptCustomHolSubmitBtn}>
                        <span>Submit</span>
                    </div>
                </div>                
            </div>
        `
        return htmlString
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

    let addDbHolsToDom = function (data) {

        //add holiday data if there is actually any to add
        if (data) {
            for (i = 0; i < data.length; i++) {
                let hol = data[i]
                //console.log(hol)
                //finds the calendar element and updates the class depending on wether the holiday has been approved or not alongside adding data attributes to be used for canceling
                let domEl = document.querySelector(`[data-cal-id*="${hol.CalendarId}"]`)
                domEl.className = `cal-block-status-${hol.ApprovalId}`
                domEl.setAttribute('data-book-id', hol.BookId);
                domEl.setAttribute('data-empLink-id', hol.EmpLinkId);
                domEl.setAttribute('data-approval-id', hol.ApprovalId);

                //if Approved change time to approved
                if (hol.ApprovalId == 1) {                    
                    domEl.querySelector(DOMStrings.calBlockTime + " span").innerText = `Booked`
                }

                //if has queue position not -1 then change time block to queue pos
                if (hol.NumQueue !== -1) {
                    domEl.querySelector(DOMStrings.calBlockTime + " span").innerText = `Queue - ${hol.NumQueue}`
                }

                //if Approved change time to approved
                if (hol.ApprovalId == 2) {
                    domEl.querySelector(DOMStrings.calBlockTime + " span").innerText = `Declined`
                }

                //Change time to pending if in that stats
                if (hol.ApprovalId == 3) {
                    domEl.querySelector(DOMStrings.calBlockTime + " span").innerText = `Pending`
                }

                //if unsuffiecient hols
                if (hol.ApprovalId == 5) {
                    domEl.querySelector(DOMStrings.calBlockTime + " span").innerText = `insufficient`
                }

                if (hol.CustomTime) {
                    domEl.querySelector(DOMStrings.calBlockTime).insertAdjacentHTML('beforeend', `<span>(${hol.StartTime} - ${hol.EndTime})</span>`)



                }

            }
        }
        
    }

    //gets the day suffix for any day given e.g 28 => 28th
    let getDteSuffix = function (day) {
        let suffix = "th";

        if (day == 1 || day == 21 || day == 31) {
            suffix = "st";
        }
        else if (day == 2 || day == 22) {
            suffix = "nd";
        }
        else if (day == 3 || day == 23) {
            suffix = "rd";
        }

        return suffix;
    }

    let cellSelectAndHeadingUpdate = function (data, dteSelect) {
        let weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

        //create a date obj wioth elem data
        let dayVal = parseInt(document.querySelector(`[data-cal-id*="${data}"] ${DOMStrings.calBlockDate}`).innerText)        
        let dte = new Date(dteSelect.year, dteSelect.month - 1, dayVal)

        //figure out what day of the week it is in
        let dteHeading = weekday[dte.getDay()] + " " + dayVal + getDteSuffix(dayVal); 

        //update the date heading
        //document.querySelector(DOMStrings.calDteHeading).innerText = dteHeading;

        //add cell select color confirmation        
        let domEl = document.querySelector(`[data-cal-id*="${data}"]`) 
        domEl.style.backgroundColor === "rgba(210, 187, 64, 0.68)" ? domEl.style.backgroundColor = "#1b4e5dcf" : domEl.style.backgroundColor = "#d2bb40ad"

    }

    return {

        setupDateHeading: function (data) {
            //updates month and year span tags with date
            setupDateSelect(data)
        },

        setupCalendarDays : function(data) {
            document.querySelector(DOMStrings.calendarContainer).insertAdjacentHTML('beforeEnd', dayTemplate(data));
        },

        setupHolidayAllocation: function (data) {
            document.getElementById(DOMStrings.holAvailableVal).innerText = Math.round((data.minutesAvailable / 60) * 100) / 100;
            document.getElementById(DOMStrings.holUsedVal).innerText = Math.round((data.minutesUsed / 60) * 100) / 100;
        },

        getDomStrings: function () {
            return DOMStrings;
        },

        setupCellForHolReq: function (data, dteSelect) {
            cellSelectAndHeadingUpdate(data, dteSelect)            
        },

        setupDBHols: function (data) {
            addDbHolsToDom(data) 
        },

        setupDBHolCancellation: function (data,el) {
           return holidayCancellationHTML(data,el)
        },

        setupRequestOveride: function (data, el) {
            return requestOverideHTML(data, el)
        },

        setupPendingRequest: function (data, el) {
            return requestPendingHTML(data, el)
        },

        setupCustomHolRequest: function (data) {
            let domEl = document.querySelector(`[data-cal-id="${data}"]`);            
            return requestCustomHolHTML(domEl)
        },

        setupDateChangeRequest: function (data, el) {
            return dateChangeHTML()
        }

    }

})();


//controller will hook up everyhthing that is required 
var controller = (function(dataCtrl, UICtrl, apiCtrl) {

    let domStrings = UIController.getDomStrings()

    //these listeners only needs to be setup once
    document.querySelector(domStrings.bookConfirm).addEventListener("click", e => sendHolidayRequest())
    document.querySelectorAll(domStrings.dateChangeBtn).forEach(e => {
        e.addEventListener('click', ev => {
            e.classList.contains(domStrings.prevMonthBtn) ? updateDateSelection(false) : updateDateSelection(true)
        });
    });
    document.querySelector(domStrings.curDatePrompt).addEventListener("click", e => setupDateSelectPrompt());    
  
    function setupEventListeners() {

        //if active then can still be assigned a holiday
        document.querySelectorAll(domStrings.calBlockActive).forEach(e => {
            e.addEventListener('mousedown', ev => {
                console.log(ev.button)
                //left click full day holiday, right click custom holiday
                ev.button === 0 ? setupHolidayRequest(e) : requestCustomHoliday(e)              
            });           
        });     

        //if status then hol has been approved and cancel option is required
        document.querySelectorAll(domStrings.calBlockApproved).forEach(e => {
            e.addEventListener('click', ev => {                
                cancelHolidayPrompt(e)
            });
        });     

        document.querySelectorAll(domStrings.calBlockInsufficient).forEach(e => {
            e.addEventListener('click', ev => {
                cancelHolidayPrompt(e)
            });
        });     

        document.querySelectorAll(domStrings.calBlockQueue).forEach(e => {
            e.addEventListener('click', ev => {
                requestOveridePrompt(e)
            });
        });

        document.querySelectorAll(domStrings.calBlockPending).forEach(e => {
            e.addEventListener('click', ev => {
                requestPendingPrompt(e)
            });
        });        
    }    

    function updateDateSelection(next) {

        //get the current date selected
        let dteChosen = dataCtrl.getCalDateSelected();
        let yr = dteChosen.year;
        let month = dteChosen.month;
        let fullDayMins = 0;

        //if true going forward one month
        next ? month += 1 : month-=1

        //if going forward a month and and is on 13 them month needs to be changed to 1 and the year incremented
        if (month === 13) {
            month = 1
            yr +=1
        }

        //if going back a month and month is 0 them month needs to be changed to 12 and the year decrement
        if (month === 0) {
            month = 12
            yr -= 1
        }

        //call data update the intialize to new dates 
        dataCtrl.updateCalDate(month,yr)
        init();
       
    }
    
    function setupDateSelectPrompt() {
        let promptEl = UICtrl.setupDateChangeRequest()      
        popupComponent.popup(promptEl, [
            {
                el: "." + domStrings.dateChangeSubmit,
                event: function () {
                    dataCtrl.updateCalDate(parseInt(document.querySelector(domStrings.dateInputMonth).value), parseInt(document.querySelector(domStrings.dateInputYear).value));
                    init();
                }
            },
        ]); 
    }

    //when cal block is clicked will highlight and add the calendar id to 
    function setupHolidayRequest(element) {
        // -1 means does not exist
        let calID = element.dataset.calId;
        let fullDayMins = dataCtrl.getUserInfoObj().fullDayAllocation
        let holRequest = dataCtrl.CheckForHoliday(calID);
        if (holRequest > -1) {
            dataCtrl.removeHolidayRequest(holRequest);
            dataCtrl.updateUserInfoObj(fullDayMins, false);
        } else {
            dataCtrl.addHolidayRequest(calID,fullDayMins,false,"-1","-1");            
            dataCtrl.updateUserInfoObj(fullDayMins, true);
        }

        UICtrl.setupCellForHolReq(calID,dataCtrl.getCalDateSelected());       
        UICtrl.setupHolidayAllocation(dataCtrl.getUserInfoObj());
    }

    let getPromptData = function (el) {
        let ob = {
            calID: el.dataset.calId,
            dataObj: dataCtrl.getDBHolFromList(el.dataset.calId)
        }
        return ob        
    }

    let cancelHoliday = function (data) {
        apiCtrl.cancelHoliday(data).then(e => {
            init();
            popupComponent.close();
        }).catch(function (err) {
            alert(err);
        });
    }

    function cancelHolidayPrompt(el) {
        ob = getPromptData(el)
        let promptEl = UIController.setupDBHolCancellation(ob.dataObj, el)
        popupComponent.popup(promptEl, [
            {
                el: "." + domStrings.promptCancel,
                event: function () {
                    cancelHoliday(ob.dataObj)
                }
            },
        ]);
    }

    let requestOveride = function (data) {
        data.Reason = document.getElementById(domStrings.promptOverideInput).value
        apiCtrl.requestOveride(data).then(e => {
            init();
            popupComponent.close();
        }).catch(function (err) {
            alert(err);
        });
    }

    function requestOveridePrompt(el) {
        ob = getPromptData(el);
        let promptEl = UIController.setupRequestOveride(ob.dataObj, el)
        popupComponent.popup(promptEl, [
            {
                el: "." + domStrings.promptOverideBtn,
                event: function () {
                    requestOveride(ob.dataObj)
                }
            },
            {
                el: "." + domStrings.promptCancel,
                event: function () {
                    cancelHoliday(ob.dataObj)
                }
            }
        ]);
    }

    function requestPendingPrompt(el) {
        ob = getPromptData(el);
        let promptEl = UICtrl.setupPendingRequest(ob.dataObj, el)
        popupComponent.popup(promptEl, [         
            {
                el: "." + domStrings.promptCancel,
                event: function () {
                    cancelHoliday(ob.dataObj)
                }
            }
        ]);
    }

    function setupCustomHolidayRequest(calendarID) {
        // work out the amount of mins requested
        let calID = calendarID;
        let startTime = document.getElementById(domStrings.customStartTime).value
        let endTime = document.getElementById(domStrings.customEndTime).value
        let stSplit = startTime.split(':')
        let stMins = (+stSplit[0]) * 60 + (+stSplit[1])
        let edSplit = endTime.split(':')
        let edMins = (+edSplit[0]) * 60 + (+edSplit[1])
        let minReq = edMins - stMins

        //setupRequest
        let holRequest = dataCtrl.CheckForHoliday(calID);
        if (holRequest === -1) {
            dataCtrl.addHolidayRequest(calID, minReq, true, startTime, endTime);
            dataCtrl.updateUserInfoObj(minReq, true);
            UICtrl.setupCellForHolReq(calID, dataCtrl.getCalDateSelected());
            UICtrl.setupHolidayAllocation(dataCtrl.getUserInfoObj());
        }
    }


    function requestCustomHoliday(el) {
        let promptEl = UICtrl.setupCustomHolRequest(el.dataset.calId)
        popupComponent.popup(promptEl, [
            {
                el: "." + domStrings.promptCustomHolSubmitBtn,
                event: function () {
                    setupCustomHolidayRequest(el.dataset.calId)
                }
            }
        ]);
    }



    //getDBHolFromList


    //makes api request to request the holiday
    function sendHolidayRequest() {
        apiCtrl.requestHoliday(dataCtrl.getHolidayRequests()).then(function () {
            init();
        }).catch(function (err) {
            alert(err)
        });;        
    }

    let init = function () {
        let dteChosen = dataCtrl.getCalDateSelected();        
        document.querySelector(".calendar-container").innerHTML = "";
        dataCtrl.clearAllData();
        apiCtrl.getCalendarData(dteChosen.month, dteChosen.year).then(function (data) {
            dataCtrl.setCal(data)
            UICtrl.setupCalendarDays(dataCtrl.getCalendarData())           
            console.log(dataCtrl.getCalendarData());
        }).then(function (result) {
            return apiCtrl.getUserHolidaysForMonth(dteChosen.month, dteChosen.year);
        }).then(function (result) {
            console.log(result);
            dataCtrl.setDBHol(result)
            console.log(dataCtrl.getDBHols())
            UICtrl.setupDBHols(dataCtrl.getDBHols());
            setupEventListeners();            
            UICtrl.setupDateHeading(dteChosen);
            return apiCtrl.getUserInfo()
        }).then(function (result) {            
            dataCtrl.setUserInfoObj(result);
            console.log(dataCtrl.getUserInfoObj())
            UICtrl.setupHolidayAllocation(dataCtrl.getUserInfoObj());
        }).catch(function (err) {
            alert(err)
        });
    }


    return {

        initialize: function (month, year) {
            dataCtrl.setSelectedDate(month, year);
            init();            
        }

    }

})(dataController,UIController,apiController);

var dte = new Date(Date.now())
controller.initialize(dte.getMonth() + 1, dte.getFullYear());

