var dataController = (function () {

    //hold values for actions completed
    class RecentActions {
        constructor(approved, declined, pending) {
            this.approved = approved;
            this.declined = declined;
            this.pending = pending
        }
    }

    let recentActions = new RecentActions(0, 0, 0);

    return {

        getRecentAction: function () {
            return recentActions;
        },

        setRecentActions: function (approved, declined, pending) {
            this.recentActions.approved = approved;
            this.recentActions.declined = declined;
            this.recentActions.pending = pending;
        }
    }

})()

var UIController = (function () {


    let DOMStrings = {
        reqUserPanel: ".request-panel-parent",
        reqParent: ".request-item-parent",
        reqContent: ".req-content",
        reqResponse: ".req-content-response",
        reqOptions: ".req-options",
        reqActionBtns: ".rec-action-btn",
        reqApprove: ".req-action-approve",
        reqDecline: ".req-action-decline"

    }

    return {

        getDomStrings: function () {
            return DOMStrings;
        },

    }


})()

var controller = (function (dataCtrl, UICtrl, apiCtrl) {

    let domStrings = UIController.getDomStrings()

    document.querySelectorAll(domStrings.reqActionBtns).forEach(e => {
        e.addEventListener('click', ev => {
            let reqParent = e.closest(domStrings.reqParent);            
            processReqAction(reqParent.dataset.bookId, reqParent.dataset.empId,reqParent.dataset.calId, reqParent);            
        });
    });

    //cancel holiday and remove from dom
    function processReqAction(bookId, empId, calId, reqEl) {
        apiCtrl.cancelHoliday({ CalendarId: calId, BookId: bookId, EmpLinkId: empId }).then(e => {
            reqEl.parentElement.removeChild(reqEl)
        }).catch(function (err) {
            alert(err);
        });


        //apiCtrl.StaffRequestVerdict(bookId, approved, response).then(e => {
        //    //req item and the user poanel if none remaining
        //    let userPanel = reqEl.closest(domStrings.reqUserPanel);
        //    if (userPanel.querySelectorAll(domStrings.reqParent).length > 1) {
        //        reqEl.parentElement.removeChild(reqEl);
        //    } else {
        //        userPanel.parentElement.removeChild(userPanel);
        //    }
        //}).catch(e => {
        //    alert("utter failure");
        //})
    }


    function init() {

    }

    return {

        initialize: function () {
            init();
        }

    }


})(dataController, UIController, apiController)

controller.initialize();
