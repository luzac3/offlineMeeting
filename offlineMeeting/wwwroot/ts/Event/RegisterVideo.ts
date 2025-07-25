import { FetchApi } from "@root/share/FetchApi";
import { StartVideoMedia } from "./StartVideoMedia";
import { GetCameraInformation } from "./GetCameraInformation";

export class RegisterVideo {
    cameraContainer: { [key: string]: string }[] = [];
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    registerVideoProperty: {
        [key: string]: string | null
    } = {
            DeviceId: null,
            Label: null,
            CameraName: null
    };

    constructor() {
        this.url = '/Event/RegisterVideo';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";
    }

    setRegisterVideoEvent = async () => {
        let videoElement: HTMLVideoElement;

        document.getElementById("getRegisterVideoSelectorButton")?.addEventListener("click", async () => {
            // 接続済カメラデバイスを取得、クラスPropertyにIDをセット
            this.cameraContainer = await GetCameraInformation.setDevices();

            // カメラ選択optionElementをセット
            let cameraSelector = document.getElementById("registerVideoList") as HTMLSelectElement;

            GetCameraInformation.createCameraSelectorObject(cameraSelector);

            // カメラを取得、Optionにセットしたら画面を表示
            document.getElementById("startRegisterVideoButton")!.classList.remove("hidden");
        });

        document.getElementById("startRegisterVideoButton")?.addEventListener("click", async () => {
            const nowCamera = document.getElementById("registerVideoList") as HTMLSelectElement;
            const startVideoMedia = new StartVideoMedia();
            videoElement = await startVideoMedia.setVideo(
                nowCamera.value,
                "registerVideo",
                320,
                240
            );
        });

        // ビデオ停止
        document.getElementById("stopRegiterVideoButton")?.addEventListener("click", async () => {
            videoElement.pause();
            videoElement.load();
            const source = videoElement.querySelector("source");
            if (source != null) {
                source.src = "";
                videoElement.load();
            }
        });

        // ビデオ登録
        document.getElementById("registerVideoInformationButton")?.addEventListener("click", async () => {
            const cameraElement = document.getElementById("registerVideoList") as HTMLSelectElement;
            const selectedIndex = cameraElement.selectedIndex;
            const cameraNameElement = document.getElementById("registerCameraName") as HTMLInputElement;

            this.registerVideoProperty.CameraName = cameraNameElement.value;
            this.registerVideoProperty.DeviceId = cameraElement.value;
            this.registerVideoProperty.Label = cameraElement.options[selectedIndex].text;

            this.setRegisterVideo();
        });
    }

    private setRegisterVideo = async (): Promise<void> => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.registerVideoProperty,
            this.responseKind
        ).then(async (data: boolean) => {
            if (data) {
                alert("register complete");
            }
        }).catch(e => {
            throw e;
        });
    }
}