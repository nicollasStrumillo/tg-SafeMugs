import { ChangeDetectionStrategy, Component, ElementRef, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'sm-footer',
  imports: [RouterLink],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterComponent {
  protected readonly currentYear = new Date().getFullYear();

  private readonly host = inject(ElementRef);

  protected openGitHub(event: MouseEvent): void {
    event.preventDefault();
    // Parte da vulnerabilidade de DOM Clobbering
    const url: string | null = this.host.nativeElement.ownerDocument.getElementById('githubLink')?.getAttribute('href');

    window.location.href = url || 'https://github.com/nicollasStrumillo/tg-SafeMugs';
  }
}

