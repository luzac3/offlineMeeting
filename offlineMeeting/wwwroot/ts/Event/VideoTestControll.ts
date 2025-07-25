import { StartVideoMedia } from "./StartVideoMedia";

export class VideoTestControll {
    cameraContainer: { [key: string]: string }[] = [];
    fps: number = 60;
    intervalId: number = 0;

    setTestVideoEvent = async () => {

        let videoElement: HTMLVideoElement;

        document.getElementById("getTestVideoSelector")?.addEventListener("click", async () => {
            // 接続済カメラデバイスを取得、クラスPropertyにIDをセット
            await this.setDevices();

            // カメラ選択optionElementをセット
            let cameraSelector = document.getElementById("testVideoList") as HTMLSelectElement;

            this.createCameraSelectorObject(cameraSelector);

            // カメラを取得、Optionにセットしたら画面を表示
            document.getElementById("startTestVideo")!.classList.remove("hidden");
        });

        document.getElementById("startTestVideo")?.addEventListener("click", async () => {
            const nowCamera = document.getElementById("testVideoList") as HTMLSelectElement;
            const startVideoMedia = new StartVideoMedia();
            videoElement = await startVideoMedia.setVideo(
                nowCamera.value,
                "testVideo",
                200,
                150
            );
        });

        document.getElementById("endVideoTest")?.addEventListener("click", async () => {
            videoElement.pause();
            videoElement.load();
            const source = videoElement.querySelector("source");
            if (source != null) {
                source.src = "";
                videoElement.load();
            }
        })
    }
    
    private setDevices = async () => {
        const devices = (await navigator.mediaDevices.enumerateDevices());
        this.cameraContainer = [];
        devices.forEach(device => {
            if (device.kind !== "videoinput") {
                return;
            }
            this.cameraContainer.push(
                {
                    deviceId: device.deviceId,
                    label: device.label
                }
            )
        });
    }

    private createCameraSelectorObject = (selectObject: Element) => {
        if (selectObject.childElementCount > 0) {
            Array.from(selectObject.children).forEach((element: Element) => {
                element.remove();
            });
        }
        this.cameraContainer.forEach((camera) => {
            const cameraOptionElement = document.createElement("option");
            cameraOptionElement.value = camera.deviceId;
            cameraOptionElement.text = camera.label;
            selectObject.appendChild(cameraOptionElement);
        });
    }
}