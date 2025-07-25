import { SetCanvas } from "./SetCanvas"

export class VideoSwitcher {
    setSwitchEvent = (setCanvas: SetCanvas) => {
        const selectVideos = document.getElementsByClassName("selectVideo") as HTMLCollectionOf<HTMLInputElement>;
        const clickVideos = document.getElementsByClassName("videoWrapper") as HTMLCollectionOf<HTMLElement>;
        this.setSelectedColor(selectVideos.item(0) as HTMLInputElement);

        Array.from(selectVideos).forEach(selectVideo => {
            selectVideo.addEventListener("change", () => {
                this.changeVideo(setCanvas);
            });
        });

        Array.from(clickVideos).forEach(clickVideo => {
            clickVideo.addEventListener("click", () => {
                this.checkRadio(clickVideo);
            });
        });
    }

    private checkRadio = (clickVideo: HTMLElement) => {
        const selectVideo = clickVideo.querySelector(".selectVideo") as HTMLInputElement;
        const changeEvent = new Event("change");
        selectVideo.checked = true;
        selectVideo.dispatchEvent(changeEvent);
    }

    private changeVideo = (setCanvas: SetCanvas) => {
        const videoElement = 
            Array.from(
                <HTMLCollectionOf<HTMLInputElement>>document.getElementsByClassName("selectVideo")
            )
            .filter(v => v.checked)
            .map((v, i) => v.closest(".videoWrapper")?.querySelector("video"))[0] as HTMLVideoElement;

        this.setSelectedColor(videoElement);

        setCanvas.setVideoElement(videoElement);
    };

    private setSelectedColor = (selectedElement: HTMLElement) => {
        const selectedClasses = document.getElementsByClassName("selected") as HTMLCollectionOf<HTMLElement>;;
        Array.from(selectedClasses).forEach((selectedClass) => {
            selectedClass.classList.remove("selected");
        });

        selectedElement.closest(".videoWrapper")?.classList.add("selected");
    }
}