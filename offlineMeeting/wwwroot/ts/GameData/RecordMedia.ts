import { FetchApi } from "@root/share/FetchApi";

export class RecordMedia {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    isStopRecording: boolean = false;
    recordingInterval: number = 0;
    recordBlobs: RecordBlob[] = [];

    constructor(
        url: string
    ) {
        this.url = url;
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";
    } 

    startRecording = (
        eventNumber: string,
        gameNumber: string,
        handNumber: string,
        handSubNumber: string,
        mediaMinutes: string,
        ...mediaAssociates: {[key: string]: (MediaStream | string | boolean | null)}[]
    ) => {
        this.recordBlobs = mediaAssociates
            .map(mediaAssociate => new RecordBlob(
                eventNumber,
                gameNumber,
                handNumber,
                handSubNumber,
                mediaMinutes,
                <string>mediaAssociate.MediaNumber,
                <MediaStream>mediaAssociate.MediaStream,
                <boolean>mediaAssociate.IsVideo
            ));

        this.recordBlobs.forEach(recordBlob => {
            recordBlob.recorder(this.encodeBlob);
        });

        this.recordingInterval = window.setInterval(
            () => {
                this.recordBlobs.forEach(recordBlob => {
                    recordBlob.mediaRecorder?.stop();
                    recordBlob.mediaRecorder?.start();
                    this.encodeBlob(recordBlob);
                });
            },
            1000 * Number(mediaMinutes)
        );
    }

    stopRecording = () => {
        clearInterval(this.recordingInterval);
        this.recordBlobs.forEach(recordBlob => {
            recordBlob.isStopRecording = true;
            recordBlob.mediaRecorder?.stop();
        });
        return this.recordBlobs.length;
    }

    private encodeBlob = ( recordBlob: RecordBlob ) => {
        let recordedMediaProperty = recordBlob.recordedMediaProperty;
        // fixme メソッド呼び出しにして直接書き換えないようにする
        if (recordBlob.blob?.length ?? 0 > 0) {
            const inBlob = new Blob(recordBlob.blob, { type: recordBlob.blob[0].type });
            // 保存した時点で、Blobの中身を消去
            recordBlob.blob = [];

            const fr = new FileReader();
            fr.readAsDataURL(inBlob);
            fr.onload = () => {
                const result = fr.result as string;
                if (!result) {
                    throw new Error();
                }
                const base64 = result.slice(result.indexOf(',') + 1);

                recordedMediaProperty.MediaCount = recordBlob.mediaCount.toString();
                recordedMediaProperty.Base64 = base64;
                recordBlob.mediaCount++;

                this.sendBlob(recordedMediaProperty).then(() => {
                    console.log("arrival");
                }).catch(e => {
                    // fixme 最終的にはエラーログを吐く
                    console.log(e);
                    throw e;
                });
            }
        }
    }

    private sendBlob = async (recordedVideoProperty: { [key: string]: string | boolean | null }) => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            recordedVideoProperty,
            this.responseKind
        ).then(async (data: boolean) => {
            if (data) {
            }
        }).catch(e => {
            throw e;
        });
    }
}

class RecordBlob {
    isStopRecording: boolean = false;
    recordedMediaProperty: { [key: string]: string | boolean | null };
    mediaCount: number = 0;
    isVideo: boolean = false;
    mediaRecorder?: MediaRecorder;
    mediaStream: MediaStream;
    recordingInterval?: number;
    blob: Blob[] = [];

    constructor(

        eventNumber: string,
        gameNumber: string,
        handNumber: string,
        handSubNumber: string,
        mediaMinutes: string,
        mediaNumber: string | null,
        mediaStream: MediaStream,
        isVideo: boolean
    ) {
        this.recordedMediaProperty = {
            EventNumber: eventNumber,
            GameNumber: gameNumber,
            HandNumber: handNumber,
            HandSubNumber: handSubNumber,
            MediaNumber: mediaNumber,
            MediaCount: "",
            MediaMinutes: mediaMinutes.toString(),
            Base64: "",
            IsVideo: isVideo,
            IsEndFile: false
        };
        this.isVideo = isVideo;
        this.mediaStream = mediaStream;
    }

    recorder = (encodeBlob: (recordBlob: RecordBlob) => void) => {
        const mediaType = this.isVideo ? "video" : "audio";
        const mime = MediaRecorder.isTypeSupported(`${mediaType}/webm; codecs=vp9`) ?
            `${mediaType}/webm; codecs=vp9` :
            `${mediaType}/webm`;

        this.mediaRecorder = new MediaRecorder(this.mediaStream, { mimeType: mime });
        this.blob = [];

        // 記録開始
        this.mediaRecorder.ondataavailable = (e: BlobEvent) => {
            (this.blob).push(e.data);
            if (this.isStopRecording) {
                this.recordedMediaProperty.IsChangeFile = true;
                encodeBlob(this);
            }
        }

        this.mediaRecorder.start();
    }
}