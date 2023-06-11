import { Component, Input, OnInit } from '@angular/core';
import { EmailSettings } from 'src/models/emailSettings';
import { EmailSettingsService } from 'src/services/emailSettings.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inputs-block',
  templateUrl: './inputs-block.component.html',
  styleUrls: ['./inputs-block.component.scss']
})
export class InputsBlockComponent implements OnInit {
  emailSettings: EmailSettings = {
    intervalValue: 0, 
    startTime: 0,
    endTime:0,
    isScheduled: false,
    periodicity: '', 
    daysOfWeek: []
  };

  options: string[] = ['hours', 'minutes', 'seconds']; 

 constructor(private emailSettingService: EmailSettingsService, 
  private formBuilder: FormBuilder,
  private toastr: ToastrService){}

  ngOnInit(): void {
   this.emailSettingService.getGeneralProperties().subscribe({
    next: (response: any) => {
      this.emailSettings = response,
      console.log(this.emailSettings)
    },
    error: error => console.log(error)
    });
 }

 showSuccessNotification(){
  this.toastr.success('Job updated successfully!', 'Success')
 }

 onCheckboxDaysChange(additionalValue: number) {
  if (this.emailSettings.daysOfWeek.includes(additionalValue)) {
    const index = this.emailSettings.daysOfWeek.indexOf(additionalValue);
    if (index > -1) {
      this.emailSettings.daysOfWeek.splice(index, 1);
    }
  } else {
    this.emailSettings.daysOfWeek.push(additionalValue);
  }
  }

  onIsScheduledChange(){
    this.emailSettings.isScheduled = !this.emailSettings.isScheduled;
    console.log('Checkbox checked:', this.emailSettings.isScheduled);
  }

  onFormSubmit() {
    this.emailSettingService.setNewEmailSettings(this.emailSettings).subscribe(
      (response: any) => {
        // Handle successful response
        this.showSuccessNotification();
        this.emailSettings = response;
      },
      (error: any) => {
        // Handle error
        console.error('An error occurred:', error);
      });
      
  }
  
  onChange(event: any) {
    this.emailSettings.periodicity = event.target.value;
  }
  
  cancelChanges(){
    this.emailSettingService.getGeneralProperties().subscribe({
      next: (response: any) => {
        this.emailSettings = response,
        console.log(this.emailSettings)
      },
      error: error => console.log(error)
      });
  }
  
}
