import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({ selector: 'messagebox-component', templateUrl: 'messagebox-component.html' })
export class MessageboxComponent {

    constructor(
        public dialogRef: MatDialogRef<MessageboxComponent>,
        @Inject(MAT_DIALOG_DATA) public data: MessageboxData) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

}

export interface MessageboxData {
    title: string;
    message: string;
}