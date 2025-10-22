export class ManagedCSV {
    read = () => {

    }

    write = async (headers: string[], rows: string[][], fileName: string) => {
        const csvRows = [headers.join(",")];
        csvRows.concat(rows.map(x => x.join(",")));

        if (!("showDirectoryPicker" in window)) {
            alert("File System Access APIはこのブラウザでサポートされていません。");
            return;
        }
        try {
            const dirHandle = await(window as any).showDirectoryPicker();
            const fileHandle = await dirHandle.getFileHandle(fileName, { create: true });
            const writable = await fileHandle.createWritable();
            await writable.write(csvRows);
            await writable.close();
            alert("CSVファイルを保存しました。");
        } catch (e) {
            alert("CSV保存に失敗しました: " + e);
        }
        return csvRows.join("\r\n");
    }
}