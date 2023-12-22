import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model: any = {};
  constructor(private authService: AccountService, private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigateByUrl("/");
    }
  }

  login(form: NgForm) {
    form.control.markAllAsTouched();
    if (form.valid) {
      this.authService.login(form.value).subscribe({
        next: (response) => {
          localStorage.setItem("user", JSON.stringify(response));
          console.log(localStorage.getItem("user"));          
          this.router.navigateByUrl("/welcome");
          setTimeout(() => {
            alert("Accesso effettuato");
          }, 200);
        },
      });
    }
  }

}
