import Hls from "hls.js";
import { SignalR } from "@root/share/SignalR";

export class StartStreaming {
    EventNumber: string;
    GameNumber: string;
    HandNumber: string;
    HandSubNumber: string;

    constructor() {
        this.EventNumber = (<HTMLInputElement>document.getElementById("eventNumber")).value;
        this.GameNumber = (<HTMLInputElement>document.getElementById("gameNumber")).value;
        this.HandNumber = (<HTMLInputElement>document.getElementById("handNumber")).value;
        this.HandSubNumber = (<HTMLInputElement>document.getElementById("handSubNumber")).value;
    }

    setStreaming = (signalR: SignalR) => {
        // アクセスした時点からmediaを呼び出す
        this.activateMedia(signalR);
    }

    private activateMedia = (signalR: SignalR) => {
        signalR.get(
            "SendMediaProperty",
            json => {
                json = json as { [key: string]: string | number | boolean }
                const media =
                    json["isVideo"] ?
                    document.getElementById("videoStreaming") as HTMLVideoElement :
                    document.getElementById("audioStreaming") as HTMLAudioElement;
                const mediaSrc = json["m3u8FilePath"] as string;

                this.showMedia(media, mediaSrc);
            }
        );

        signalR.send(
            "GetIndex",
            Number(this.EventNumber),
            Number(this.GameNumber),
            Number(this.HandNumber),
            Number(this.HandSubNumber),
            "streaming/",
            true,
            0
        );

        signalR.send(
            "GetIndex",
            Number(this.EventNumber),
            Number(this.GameNumber),
            Number(this.HandNumber),
            Number(this.HandSubNumber),
            "streaming/",
            false,
            0
        );
    };

    private showMedia = (
        mediaElement: HTMLVideoElement | HTMLAudioElement,
        mediaSrc: string
    ) => {
        if (Hls.isSupported()) {
            var hls = new Hls();
            hls.loadSource(mediaSrc);
            hls.attachMedia(mediaElement);
        }
        else if (mediaElement.canPlayType('application/vnd.apple.mpegurl')) {
            mediaElement.src = mediaSrc;
        }
        /*
            timerテスト用
            const video = document.getElementById("video_0") as HTMLVideoElement;
            const timer = document.getElementById("timer") as HTMLElement;
            timer.textContent = video.currentTime.toString();

        */

        mediaElement.play();
    }
}