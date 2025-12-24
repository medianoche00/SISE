import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  form: FormGroup;
  loading = false;
  returnUrl = '/';

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private snack: MatSnackBar
  ) {
    this.form = this.fb.group({
      userNameOrEmail: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });

    const q = this.route.snapshot.queryParamMap.get('returnUrl');
    if (q) this.returnUrl = q;
  }

  submit() {
    if (this.form.invalid) return;
    this.loading = true;
    const { userNameOrEmail, password } = this.form.value;
    this.auth.login(userNameOrEmail, password).subscribe({
      next: () => {
        this.loading = false;
        this.snack.open('Inicio de sesión correcto', 'Cerrar', { duration: 2000 });
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error?.message ?? 'Credenciales incorrectas';
        this.snack.open(msg, 'Cerrar', { duration: 3000 });
      }
    });
  }
}