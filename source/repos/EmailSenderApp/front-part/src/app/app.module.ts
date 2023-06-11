import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { InputsBlockComponent } from './inputs-block/inputs-block.component';
import { HttpClientModule } from '@angular/common/http';

import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    InputsBlockComponent
  ],
  imports: [
    BrowserModule, 
    HttpClientModule,
    FormsModule, 
    ToastrModule.forRoot({
      timeOut: 3000, // Adjust the timeout duration in milliseconds
  extendedTimeOut: 0, // Set extended timeout to 0 to disable it
  easeTime: 300, // Adjust the animation ease time in milliseconds
  progressBar: true, // Enable or disable the progress bar animation
  progressAnimation: 'increasing', // Set the progress bar animation style
  positionClass: 'toast-top-center'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
