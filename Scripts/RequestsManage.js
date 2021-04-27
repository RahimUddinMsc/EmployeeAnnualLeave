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
        reqUserPanel:".request-panel-parent",
        reqParent:".request-item-parent",
        reqContent: ".req-content",
        reqResponse: ".req-content-response",
        reqOptions: ".req-options",
        reqActionBtns: ".rec-action-btn",
        reqApprove: ".req-action-approve",
        reqDecline: ".req-action-decline"
       
    }

    let eliminateVoidDoms = function () {
        document.querySelectorAll(DOMStrings.reqUserPanel).forEach(e => {
            let check = e.querySelector(DOMStrings.reqParent)
            if (!check) {
                e.parentElement.removeChild(e);
                return;
            }

        });
    }

    return {

        getDomStrings: function () {
            return DOMStrings;
        },

        removeDomEls : function() {
            eliminateVoidDoms();
        }

    }


})()

var controller = (function (dataCtrl, UICtrl, apiCtrl) {

    let domStrings = UIController.getDomStrings()

    document.querySelectorAll(domStrings.reqActionBtns).forEach(e => {
        e.addEventListener('click', ev => {
            let reqParent = e.closest(domStrings.reqParent);
            let bookId = reqParent.dataset.bookId
            let reqResponse = reqParent.querySelector(domStrings.reqResponse + ' textarea').value;
            e.matches(domStrings.reqApprove) ? processReqAction(bookId, true, reqResponse,reqParent) : processReqAction(bookId,false, reqResponse,reqParent)
        });
    });

    function processReqAction(bookId, approved, response,reqEl) {
        apiCtrl.StaffRequestVerdict(bookId, approved, response).then(e => {
            //req item and the user poanel if none remaining
            let userPanel = reqEl.closest(domStrings.reqUserPanel);
            if (userPanel.querySelectorAll(domStrings.reqParent).length > 1) {
                reqEl.parentElement.removeChild(reqEl);
            } else {
                userPanel.parentElement.removeChild(userPanel);
            }
        }).catch(e => {
            alert("utter failure");
        })
    }


    function init() {
       
    }

    return {

        initialize: function () {           
            init();
            UICtrl.removeDomEls();
        }

    }


})(dataController, UIController, apiController)

controller.initialize();
