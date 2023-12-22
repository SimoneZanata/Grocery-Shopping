import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterUser } from 'src/app/models/RegisterUser';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  constructor(private authService: AccountService, private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigateByUrl("/");
    }
  }

  register(form: NgForm) {
    form.control.markAllAsTouched();
    if (form.valid) {
      const registerForm: RegisterUser = form.value;
      this.authService.register(registerForm).subscribe({
        next: () => {
          this.router.navigateByUrl("/login");
          setTimeout(() => {
            alert('Registrazione Effettuata con successo');
          }, 200);
        },
        error: (error) => {
          console.error('Errore durante la registrazione:', error);
          setTimeout(() => {
            alert('Errore durante la registrazione');
          }, 200);
        }
      });
    }
  }

}