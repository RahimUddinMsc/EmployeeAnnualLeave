const transition = document.querySelector('.user-greeting-ct');
console.log(transition)
transition.addEventListener('animationend', () => {
    transition.classList.add('greeting-ct-static');
});


