import * as React from 'react';
import styles from './Button.module.css';

export type ButtonRef = HTMLButtonElement;

export type ButtonColor = 'primary' | 'secondary' | 'success' | 'danger' | 'info' | 'link';
export type ButtonStylingMode = 'text' | 'contained' | 'outlined';

type ButtonProps = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  icon?: string;
  color?: ButtonColor;
  stylingMode?: ButtonStylingMode;
  text?: string;
};

const stylingModeClassName: Record<ButtonStylingMode, string> = {
  text: styles.text,
  contained: styles.contained,
  outlined: styles.outlined,
};

const colorClassName: Record<ButtonColor, string> = {
  primary: styles.primary,
  secondary: styles.secondary,
  success: styles.success,
  danger: styles.danger,
  info: styles.info,
  link: styles.link,
};

const Button = React.forwardRef<ButtonRef, ButtonProps>(function Button(
  { text, className, icon, color = 'primary', stylingMode = 'contained', children, ...props },
  ref
) {
  const buttonClassName = [
    styles.button,
    stylingModeClassName[stylingMode],
    colorClassName[color],
    className,
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <button
      ref={ref}
      type={props.type ?? 'button'}
      className={buttonClassName}
      {...props}
    >
      {icon ? <i className={`dx-icon dx-icon-${icon} ${styles.icon}`} aria-hidden="true" /> : null}
      {text ? <span>{text}</span> : children}
    </button>
  );
});

export default Button;
