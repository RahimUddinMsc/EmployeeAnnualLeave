var pgTransition = (function () {

    //Grab all the anchor items and prevent the default redirect
    document.querySelectorAll('.navbar-collapse a').forEach(function (e) {

        //add transtiion class to the panels required 
        e.addEventListener('click', ev => {
            ev.preventDefault();            
            document.querySelector('.body-panel-left').classList.add('pg-slide-left');            
            document.querySelector('.body-panel-right').classList.add('pg-slide-right');

            //send to new page with timeou val being lenght of tranistion
            setTimeout(() => {
                window.location.href = ev.target.href;
            },600);


        })


    });
    

})();

