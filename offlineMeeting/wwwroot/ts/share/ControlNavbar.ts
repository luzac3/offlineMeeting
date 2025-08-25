export class ControlNavbar {
    constructor() {
        this.setBodyPadding();
        window.addEventListener('resize', this.setBodyPadding);
    }

    setBodyPadding = () => {
        const navbar = document.querySelector('.navbar') as HTMLElement;
        if (navbar) {
            document.body.style.paddingTop = navbar.offsetHeight + 'px';
        }
    }
}