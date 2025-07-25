import { SignalR } from "@root/share/SignalR";
import { SetEventListner } from "@root/share/SetEventListner"
import { RecordMedia } from "@root/GameData/RecordMedia"

export class RecordVideo {
    setRecordEvent = (signalR: SignalR) => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#startRecordingButton",
            async (event) => {
                this.activateVideo(signalR, event);
            },
            true
        )
    }

    private activateVideo = async (signalR: SignalR, event: Event) => {
        const medias = document.getElementsByClassName("canvasVideo") as HTMLCollectionOf<HTMLCanvasElement>;
        const videoMinutes = 10;

        const recordMedia = new RecordMedia('/GameData/RecordedVideo');

        const mediaAssociates = Array.from(medias)
            .map((media: HTMLCanvasElement) => {
                if (media.dataset.cvenable != "0") {
                    return {
                        MediaStream: media.captureStream(),
                        MediaNumber: (<HTMLElement>media.closest(".videoWrapper")).dataset.videonumber as string,
                        IsVideo: true
                    }
                }
            }).filter(Boolean) as {[key: string]: (string | MediaStream | boolean)}[];

        recordMedia.startRecording(
            (<HTMLInputElement>document.getElementById("eventNumber")).value,
            (<HTMLInputElement>document.getElementById("gameNumber")).value,
            (<HTMLInputElement>document.getElementById("handNumber")).value,
            (<HTMLInputElement>document.getElementById("handSubNumber")).value,
            videoMinutes.toString(),
            ...mediaAssociates
        );

        // 録画停止用イベント生成
        (<HTMLButtonElement>event.target).textContent = "録画停止";
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#startRecordingButton",
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

    private getRecordingFinished = (signalR: SignalR) => {
        const recordingCommentaryElement = document.getElementById("startRecordingButton") as HTMLButtonElement;
        signalR.get("SendRecordingFinished", () => {
            recordingCommentaryElement.textContent = "記録開始";
            recordingCommentaryElement.disabled = false;

            SetEventListner.setEvent(
                document.getElementById("listModalWindow"),
                "click",
                "#startRecordingButton",
                async (event) => {
                    return this.activateVideo(signalR, event);
                },
                true
            );
        });

        // 終了処理送信
        signalR.send(
            "FinishRecording",
            (<HTMLInputElement>document.getElementById("eventNumber")).value,
            (<HTMLInputElement>document.getElementById("gameNumber")).value,
            "management/"
        );
    }
}