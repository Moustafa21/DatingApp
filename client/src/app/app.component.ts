import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AccountService } from './_services/account.service';
import { NavComponent } from "./nav/nav.component";
import { HomeComponent } from "./home/home.component";
import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, NgFor, NavComponent, HomeComponent,NgxSpinnerComponent]
})

export class AppComponent implements OnInit{
  private accountService = inject(AccountService);
  title = 'DatingApp';
  ngOnInit(): void {
    this.setCurrentUser();

  }
  setCurrentUser(){
    const userString =localStorage.getItem("user");
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

}
