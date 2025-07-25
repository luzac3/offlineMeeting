export class StartVideoMedia {
    setVideo = async (
        deviceId: string,
        videoElementId: string,
        resolutionWidth: number,
        resolutionHeight: number
    ) => {
        // video要素を取得
        const videoElement = document.getElementById(videoElementId) as HTMLVideoElement;

        // video要素にWebカメラの映像を表示させる
        return await navigator.mediaDevices.getUserMedia({
            audio: false,
            video: {
                width: { ideal: resolutionWidth },
                height: { ideal: resolutionHeight },
                deviceId: deviceId
            }
        }).then(async (stream: MediaStream) => {
            if (videoElement === null) {
                throw new Error("video_" + videoElement + "is not exist");
            }

            videoElement.srcObject = stream;
            videoElement.play();
            return videoElement;
        });
    }
}
