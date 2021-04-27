//api controller this will do things like make a call for calendar data
var apiController = (function () {

    return {
        getCalendarData: function (monthID, yearID) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: "/GetCalendarData?monthSelected=" + monthID + "&yearSelected=" + yearID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },

        getUserHolidaysForMonth: function (monthID, yearID) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: "/GetUserHolidaysForMonth?monthSelected=" + monthID + "&yearSelected=" + yearID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },


        getAllocationDaysForMonth: function (monthID, yearID) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: "GET",
                    url: "/GetAllocationDaysForMonth?monthSelected=" + monthID + "&yearSelected=" + yearID,                    
                    success: function (data) {
                        resolve(data)
                    },
                    error: function (e) {
                        console.log(e)
                        reject("failure")
                    },
                });
            });
        },

        getUserInfo: function () {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: "/GetUserInfo",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },

        requestHoliday: function (data) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: "POST",
                    url: "/RequestHoliday",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },

        requestOveride: function (data) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: 'POST',
                    url: `/RequestHolidayOveride?bookId=${data.BookId}&reason=${data.Reason}`,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },

        cancelHoliday: function (data) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: `/CancelHoliday?calendarId=${data.CalendarId}&bookId=${data.BookId}&empLinkId=${data.EmpLinkId}`,
                    type: 'DELETE',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },

        UpdateAllocationDays: function (data) {       
            return new Promise(function (resolve, reject) {
                $.ajax({
                    url: "/UpdateAllocationDays",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                    dataType: "json",                    
                    success: function (data) {
                        resolve(data)
                    },
                    error: function (e) {
                        console.log(e)
                        reject("failure")
                    },
                });
            });
        },

        StaffRequestVerdict: function (bookId,approved,reason) {
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: 'POST',
                    url: `/StaffRequestVerdict?bookId=${bookId}&approved=${approved}&reason=${reason}`,                   
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        resolve(data)
                    },
                    error: function () {
                        reject("failure")
                    },
                });
            });
        },


    }

})();
