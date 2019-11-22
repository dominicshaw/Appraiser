import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { StaffDTO } from '../generated-services';

@Component({ selector: 'year-prompt-component', templateUrl: 'year-prompt-component.html' })
export class YearPromptComponent {

    constructor(
        public dialogRef: MatDialogRef<YearPromptComponent>,
        @Inject(MAT_DIALOG_DATA) public data: YearPromptData) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

}

export interface YearPromptData {
    mode: string;
    title: string;
    year: number;
    employee: number;
    employees: StaffDTO[];
}