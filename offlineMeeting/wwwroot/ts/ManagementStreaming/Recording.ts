import { SignalR } from "@root/share/SignalR";
import { SetEventListner } from "@root/share/SetEventListner"
import { RecordMedia } from "@root/GameData/RecordMedia"

export class Recording {
    setRecordEvent = (signalR: SignalR) => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#recordingCommentary",
            async (event) => {
                this.activateStream(signalR, event);
            },
            true
        );
    }

    private activateStream = async (signalR: SignalR, event: Event) => {
        // 選択されているマイクの取得
        const myMikeSelectorElement = document.getElementById("audioSelector") as HTMLInputElement;

        // canvasの取得
        const media = document.getElementById("streamingCanvasVideo") as HTMLCanvasElement;

        // mediaStreamの取得
        const audioMediaStream = await this.getAudio(myMikeSelectorElement.value,);
        const videoMediaStream = media.captureStream();

        const mediaMinutes = 10;

        const recordMedia = new RecordMedia('/ManagementStreaming/RecordedComentary');

        const mediaAssociates = [
            {
                MediaStream: videoMediaStream,
                MediaNumber: null,
                IsVideo: true
            },
            {
                MediaStream: audioMediaStream,
                MediaNumber: null,
                IsVideo: false
            }
        ];

        recordMedia.startRecording(
            (<HTMLInputElement>document.getElementById("eventNumber")).value,
            (<HTMLInputElement>document.getElementById("gameNumber")).value,
            (<HTMLInputElement>document.getElementById("handNumber")).value,
            (<HTMLInputElement>document.getElementById("handSubNumber")).value,
            mediaMinutes.toString(),
            ...mediaAssociates
        );

        // 録音停止用イベント生成
        (<HTMLButtonElement>event.target).textContent = "記録停止";
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#recordingCommentary",
            async () => {
                (<HTMLButtonElement>event.target).disabled = true;
                const mediaCount = recordMedia.stopRecording();
                let returnedCount = 0;
                signalR.get("SendMoveFileFinished", () => {
                    // 通知が来たら完了カウントを進める
                    returnedCount++;

                    // 全コピーが終わったら終了処理を走らせる
                    if (mediaCount === returnedCount) {
                        this.getRecordingFinished(signalR);
                    }
                });

            },
            true
        );
    }

    private getAudio = async (
        deviceId: string
    ) => {
        return await navigator.mediaDevices.getUserMedia({
            audio: {
                deviceId: deviceId
            },
            video: false
        }).then(async (stream: MediaStream) => {
            return stream;
        });
    }

    private getRecordingFinished = (signalR: SignalR) => {
        const recordingCommentaryElement = document.getElementById("recordingCommentary") as HTMLButtonElement;
        signalR.get("SendRecordingFinished", () => {
            recordingCommentaryElement.textContent = "記録開始";
            recordingCommentaryElement.disabled = false;

            SetEventListner.setEvent(
                document.getElementById("listModalWindow"),
                "click",
                "#startRecordingButton",
                async (event) => {
                    return this.activateStream(signalR, event);
                },
                true
            );
        });

        // 終了処理送信
        signalR.send(
            "FinishRecording",
            (<HTMLInputElement>document.getElementById("eventNumber")).value,
            (<HTMLInputElement>document.getElementById("gameNumber")).value,
            "streaming/"
        );
    }
}
