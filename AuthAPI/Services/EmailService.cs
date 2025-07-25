using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AuthAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailVerificationAsync(string email, string verificationToken)
        {
            var subject = "?? Email Verification Required - AuthAPI";
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:7045";

            // URL encode token - for + and / characters
            var encodedToken = Uri.EscapeDataString(verificationToken);
            var verificationLink = $"{baseUrl}/api/auth/verify-email?token={encodedToken}";
            
            var htmlBody = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Verification</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
            padding: 40px 30px;
            text-align: center;
            position: relative;
        }}
        
        .header::before {{
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><circle cx=""50"" cy=""50"" r=""2"" fill=""white"" opacity=""0.1""/></svg>') repeat;
            animation: float 20s infinite linear;
        }}
        
        @keyframes float {{
            0% {{ transform: translateY(0px) rotate(0deg); }}
            100% {{ transform: translateY(-20px) rotate(360deg); }}
        }}
        
        .logo {{
            font-size: 48px;
            margin-bottom: 10px;
            filter: drop-shadow(0 4px 8px rgba(0,0,0,0.1));
        }}
        
        .header h1 {{
            color: white;
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 8px;
            text-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        
        .header p {{
            color: rgba(255, 255, 255, 0.9);
            font-size: 16px;
            font-weight: 400;
        }}
        
        .content {{
            padding: 50px 40px;
            background: #ffffff;
        }}
        
        .welcome-text {{
            font-size: 20px;
            color: #2c3e50;
            margin-bottom: 25px;
            font-weight: 600;
        }}
        
        .description {{
            font-size: 16px;
            color: #5a6c7d;
            margin-bottom: 35px;
            line-height: 1.8;
        }}
        
        .cta-container {{
            text-align: center;
            margin: 40px 0;
        }}
        
        .verify-button {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-decoration: none;
            padding: 18px 40px;
            border-radius: 50px;
            font-size: 18px;
            font-weight: 600;
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.3);
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        
        .verify-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 40px rgba(102, 126, 234, 0.4);
        }}
        
        .alternative-section {{
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin: 30px 0;
            border-left: 4px solid #667eea;
        }}
        
        .alternative-title {{
            font-size: 16px;
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 15px;
        }}
        
        .link-box {{
            background: white;
            padding: 15px;
            border-radius: 8px;
            border: 2px dashed #e9ecef;
            word-break: break-all;
            font-family: 'Courier New', monospace;
            font-size: 13px;
            color: #667eea;
        }}
        
        .info-box {{
            background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
            padding: 20px;
            border-radius: 15px;
            margin: 25px 0;
            text-align: center;
        }}
        
        .info-icon {{
            font-size: 24px;
            margin-bottom: 10px;
        }}
        
        .info-text {{
            color: #8b4513;
            font-weight: 600;
            font-size: 14px;
        }}
        
        .footer {{
            background: #2c3e50;
            color: #95a5a6;
            padding: 30px;
            text-align: center;
        }}
        
        .footer-logo {{
            font-size: 24px;
            margin-bottom: 15px;
        }}
        
        .footer-text {{
            font-size: 14px;
            line-height: 1.6;
        }}
        
        .social-links {{
            margin: 20px 0;
        }}
        
        .social-links a {{
            color: #95a5a6;
            text-decoration: none;
            margin: 0 10px;
            font-size: 18px;
        }}
        
        .debug-info {{
            background: #f1f3f4;
            padding: 15px;
            border-radius: 8px;
            margin-top: 30px;
            font-size: 12px;
            color: #6c757d;
        }}
        
        .debug-info h4 {{
            color: #495057;
            margin-bottom: 10px;
        }}
        
        @media (max-width: 600px) {{
            .email-container {{
                margin: 10px;
                border-radius: 15px;
            }}
            
            .header {{
                padding: 30px 20px;
            }}
            
            .content {{
                padding: 30px 25px;
            }}
            
            .verify-button {{
                padding: 16px 30px;
                font-size: 16px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <div class=""logo"">??</div>
            <h1>AuthAPI</h1>
            <p>Secure Authentication Platform</p>
        </div>
        
        <div class=""content"">
            <div class=""welcome-text"">
                Welcome! ??
            </div>
            
            <div class=""description"">
                Thank you for registering with AuthAPI! To activate your account and securely access all features, please verify your email address by clicking the button below.
            </div>
            
            <div class=""cta-container"">
                <a href=""{verificationLink}"" class=""verify-button"">
                    ? Verify My Email
                </a>
            </div>
            
            <div class=""info-box"">
                <div class=""info-icon"">?</div>
                <div class=""info-text"">
                    This verification link is valid for 72 hours
                </div>
            </div>
            
            <div class=""alternative-section"">
                <div class=""alternative-title"">
                    ?? Button not working?
                </div>
                <p style=""margin-bottom: 15px; color: #6c757d;"">
                    You can copy and paste the link below into your browser:
                </p>
                <div class=""link-box"">
                    {verificationLink}
                </div>
            </div>
            
            <div style=""text-align: center; margin-top: 30px; color: #6c757d;"">
                <p>If you didn't request this email, you can safely ignore it.</p>
            </div>
            
            <div class=""debug-info"">
                <h4>?? Technical Information (For Developers)</h4>
                <p><strong>Token ID:</strong> {verificationToken.Substring(0, Math.Min(10, verificationToken.Length))}...</p>
                <p><strong>Encoded Token:</strong> {encodedToken.Substring(0, Math.Min(15, encodedToken.Length))}...</p>
                <p><strong>Generated:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC</p>
            </div>
        </div>
        
        <div class=""footer"">
            <div class=""footer-logo"">??</div>
            <div class=""footer-text"">
                � 2024 AuthAPI - Secure Authentication Platform<br>
                This email was sent automatically.
            </div>
            <div class=""social-links"">
                <a href=""#"">??</a>
                <a href=""#"">??</a>
                <a href=""#"">??</a>
            </div>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, htmlBody);
        }

        public async Task SendPasswordResetAsync(string email, string resetToken)
        {
            var subject = "?? Password Reset Request - AuthAPI";
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:7045";

            // URL encode token - for + and / characters 
            var encodedToken = Uri.EscapeDataString(resetToken);
            
            // Reset link for Web
            var webResetLink = $"{baseUrl}/api/auth/reset-password?token={encodedToken}";
            
            var htmlBody = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Password Reset</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            background: linear-gradient(135deg, #ff9a9e 0%, #fecfef 50%, #fecfef 100%);
            padding: 20px;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
            padding: 40px 30px;
            text-align: center;
            position: relative;
        }}
        
        .header::before {{
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><polygon points=""50,10 90,90 10,90"" fill=""white"" opacity=""0.05""/></svg>') repeat;
            animation: pulse 3s infinite ease-in-out;
        }}
        
        @keyframes pulse {{
            0%, 100% {{ opacity: 0.05; }}
            50% {{ opacity: 0.1; }}
        }}
        
        .logo {{
            font-size: 48px;
            margin-bottom: 10px;
            filter: drop-shadow(0 4px 8px rgba(0,0,0,0.1));
        }}
        
        .header h1 {{
            color: white;
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 8px;
            text-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        
        .header p {{
            color: rgba(255, 255, 255, 0.9);
            font-size: 16px;
            font-weight: 400;
        }}
        
        .content {{
            padding: 50px 40px;
            background: #ffffff;
        }}
        
        .alert-section {{
            background: linear-gradient(135deg, #ffeaa7 0%, #fab1a0 100%);
            padding: 25px;
            border-radius: 15px;
            text-align: center;
            margin-bottom: 30px;
            border-left: 5px solid #f39c12;
        }}
        
        .alert-icon {{
            font-size: 32px;
            margin-bottom: 10px;
        }}
        
        .alert-title {{
            font-size: 20px;
            font-weight: 700;
            color: #d68910;
            margin-bottom: 8px;
        }}
        
        .alert-text {{
            color: #935116;
            font-size: 14px;
        }}
        
        .welcome-text {{
            font-size: 20px;
            color: #2c3e50;
            margin-bottom: 25px;
            font-weight: 600;
        }}
        
        .description {{
            font-size: 16px;
            color: #5a6c7d;
            margin-bottom: 35px;
            line-height: 1.8;
        }}
        
        .cta-container {{
            text-align: center;
            margin: 40px 0;
        }}
        
        .reset-button {{
            display: inline-block;
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
            color: white;
            text-decoration: none;
            padding: 18px 40px;
            border-radius: 50px;
            font-size: 18px;
            font-weight: 600;
            box-shadow: 0 10px 30px rgba(245, 87, 108, 0.3);
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        
        .reset-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 40px rgba(245, 87, 108, 0.4);
        }}
        
        .web-section {{
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin: 20px 0;
            border-left: 4px solid #6c757d;
        }}
        
        .web-title {{
            font-size: 16px;
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 15px;
        }}
        
        .token-section {{
            background: linear-gradient(135deg, #fdcb6e 0%, #e84393 100%);
            padding: 25px;
            border-radius: 15px;
            margin: 30px 0;
        }}
        
        .token-title {{
            color: white;
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .token-box {{
            background: rgba(255, 255, 255, 0.9);
            padding: 15px;
            border-radius: 10px;
            font-family: 'Courier New', monospace;
            font-size: 13px;
            word-break: break-all;
            color: #2d3436;
            margin-bottom: 20px;
        }}
        
        .instructions {{
            background: rgba(255, 255, 255, 0.1);
            padding: 15px;
            border-radius: 10px;
            color: white;
        }}
        
        .instructions h4 {{
            margin-bottom: 10px;
            font-size: 14px;
        }}
        
        .instructions ol {{
            padding-left: 20px;
        }}
        
        .instructions li {{
            margin-bottom: 5px;
            font-size: 13px;
        }}
        
        .warning-box {{
            background: linear-gradient(135deg, #ff7675 0%, #fd79a8 100%);
            padding: 20px;
            border-radius: 15px;
            margin: 25px 0;
            text-align: center;
            color: white;
        }}
        
        .warning-icon {{
            font-size: 24px;
            margin-bottom: 10px;
        }}
        
        .warning-text {{
            font-weight: 600;
            font-size: 14px;
        }}
        
        .footer {{
            background: #2c3e50;
            color: #95a5a6;
            padding: 30px;
            text-align: center;
        }}
        
        .footer-logo {{
            font-size: 24px;
            margin-bottom: 15px;
        }}
        
        .footer-text {{
            font-size: 14px;
            line-height: 1.6;
        }}
        
        .security-tips {{
            background: linear-gradient(135deg, #74b9ff 0%, #0984e3 100%);
            padding: 20px;
            border-radius: 15px;
            margin: 25px 0;
            color: white;
        }}
        
        .security-title {{
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .security-list {{
            list-style: none;
            padding: 0;
        }}
        
        .security-list li {{
            margin-bottom: 8px;
            padding-left: 25px;
            position: relative;
            font-size: 14px;
        }}
        
        .security-list li:before {{
            content: '???';
            position: absolute;
            left: 0;
        }}
        
        @media (max-width: 600px) {{
            .email-container {{
                margin: 10px;
                border-radius: 15px;
            }}
            
            .header {{
                padding: 30px 20px;
            }}
            
            .content {{
                padding: 30px 25px;
            }}
            
            .reset-button {{
                padding: 16px 30px;
                font-size: 16px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <div class=""logo"">??</div>
            <h1>AuthAPI</h1>
            <p>Secure Password Reset</p>
        </div>
        
        <div class=""content"">
            <div class=""alert-section"">
                <div class=""alert-icon"">??</div>
                <div class=""alert-title"">Password Reset Request</div>
                <div class=""alert-text"">
                    We received a password reset request for your account
                </div>
            </div>
            
            <div class=""welcome-text"">
                Hello! ??
            </div>
            
            <div class=""description"">
                We received a password reset request for your account. To reset your password, click the button below. If you didn't make this request, you can safely ignore this email.
            </div>
            
            <div class=""web-section"">
                <div class=""web-title"">?? Reset Your Password</div>
                <p style=""margin-bottom: 20px; color: #6c757d;"">
                    Click the button below to open the password reset form in your web browser:
                </p>
                <div class=""cta-container"">
                    <a href=""{webResetLink}"" class=""reset-button"">
                        ?? Reset My Password
                    </a>
                </div>
            </div>
            
            <div class=""token-section"">
                <div class=""token-title"">?? Manual Reset Token (For Developers)</div>
                <div class=""token-box"">{resetToken}</div>
                
                <div class=""instructions"">
                    <h4>?? Usage Instructions:</h4>
                    <ol>
                        <li>Use the POST /api/auth/reset-password endpoint</li>
                        <li>Send this token along with your new password</li>
                        <li>Token expires in 1 hour</li>
                    </ol>
                </div>
            </div>
            
            <div class=""warning-box"">
                <div class=""warning-icon"">?</div>
                <div class=""warning-text"">
                    This token will expire in 1 hour!
                </div>
            </div>
            
            <div class=""security-tips"">
                <div class=""security-title"">?? Security Recommendations</div>
                <ul class=""security-list"">
                    <li>Choose a strong password (8+ characters, upper/lowercase, numbers, special characters)</li>
                    <li>Never share your password with anyone</li>
                    <li>Change your password regularly</li>
                    <li>Report suspicious activity immediately</li>
                </ul>
            </div>
            
            <div style=""text-align: center; margin-top: 30px; color: #6c757d; font-size: 14px;"">
                <p>If you didn't request this password reset, your account remains secure. You can safely ignore this email.</p>
            </div>
        </div>
        
        <div class=""footer"">
            <div class=""footer-logo"">??</div>
            <div class=""footer-text"">
                � 2024 AuthAPI - Secure Authentication Platform<br>
                This email was sent automatically.
            </div>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, htmlBody);
        }

        public async Task SendAccountDeletionConfirmationAsync(string email, string deletionToken)
        {
            var subject = "??? Account Deletion Confirmation - AuthAPI";
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:7045";

            // Token? URL encode
            var encodedToken = Uri.EscapeDataString(deletionToken);
            var confirmationLink = $"{baseUrl}/api/auth/confirm-account-deletion?token={encodedToken}";
            
            var htmlBody = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Account Deletion Confirmation</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            background: linear-gradient(135deg, #ff6b6b 0%, #feca57 100%);
            padding: 20px;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
            padding: 40px 30px;
            text-align: center;
            position: relative;
        }}
        
        .logo {{
            font-size: 48px;
            margin-bottom: 10px;
            filter: drop-shadow(0 4px 8px rgba(0,0,0,0.1));
        }}
        
        .header h1 {{
            color: white;
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 8px;
            text-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        
        .header p {{
            color: rgba(255, 255, 255, 0.9);
            font-size: 16px;
            font-weight: 400;
        }}
        
        .content {{
            padding: 50px 40px;
            background: #ffffff;
        }}
        
        .warning-section {{
            background: linear-gradient(135deg, #ff7675 0%, #fd79a8 100%);
            padding: 25px;
            border-radius: 15px;
            text-align: center;
            margin-bottom: 30px;
            color: white;
        }}
        
        .warning-icon {{
            font-size: 32px;
            margin-bottom: 10px;
        }}
        
        .warning-title {{
            font-size: 20px;
            font-weight: 700;
            margin-bottom: 8px;
        }}
        
        .warning-text {{
            font-size: 14px;
        }}
        
        .description {{
            font-size: 16px;
            color: #5a6c7d;
            margin-bottom: 35px;
            line-height: 1.8;
        }}
        
        .consequences-box {{
            background: #fff5f5;
            border: 2px solid #fed7d7;
            padding: 20px;
            border-radius: 15px;
            margin: 25px 0;
        }}
        
        .consequences-title {{
            color: #c53030;
            font-weight: 600;
            font-size: 16px;
            margin-bottom: 15px;
        }}
        
        .consequences-list {{
            list-style: none;
            padding: 0;
        }}
        
        .consequences-list li {{
            color: #744210;
            margin-bottom: 8px;
            padding-left: 25px;
            position: relative;
        }}
        
        .consequences-list li:before {{
            content: '??';
            position: absolute;
            left: 0;
        }}
        
        .cta-container {{
            text-align: center;
            margin: 40px 0;
        }}
        
        .confirm-button {{
            display: inline-block;
            background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
            color: white;
            text-decoration: none;
            padding: 18px 40px;
            border-radius: 50px;
            font-size: 18px;
            font-weight: 600;
            box-shadow: 0 10px 30px rgba(231, 76, 60, 0.3);
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        
        .confirm-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 40px rgba(231, 76, 60, 0.4);
        }}
        
        .alternative-section {{
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin: 30px 0;
            border-left: 4px solid #e74c3c;
        }}
        
        .link-box {{
            background: white;
            padding: 15px;
            border-radius: 8px;
            border: 2px dashed #e9ecef;
            word-break: break-all;
            font-family: 'Courier New', monospace;
            font-size: 13px;
            color: #e74c3c;
        }}
        
        .footer {{
            background: #2c3e50;
            color: #95a5a6;
            padding: 30px;
            text-align: center;
        }}
        
        .footer-text {{
            font-size: 14px;
            line-height: 1.6;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <div class=""logo"">???</div>
            <h1>AuthAPI</h1>
            <p>Account Deletion Confirmation</p>
        </div>
        
        <div class=""content"">
            <div class=""warning-section"">
                <div class=""warning-icon"">??</div>
                <div class=""warning-title"">CRITICAL: Account Deletion Request</div>
                <div class=""warning-text"">
                    This action cannot be undone
                </div>
            </div>
            
            <div class=""description"">
                We received a request to permanently delete your AuthAPI account. This action is <strong>irreversible</strong> and will result in the complete removal of your account and all associated data.
            </div>
            
            <div class=""consequences-box"">
                <div class=""consequences-title"">?? What will happen when you confirm:</div>
                <ul class=""consequences-list"">
                    <li>Your account will be permanently deleted</li>
                    <li>All your data will be removed from our servers</li>
                    <li>You will lose access to all AuthAPI services</li>
                    <li>This action cannot be reversed or undone</li>
                    <li>You will need to create a new account to use our services again</li>
                </ul>
            </div>
            
            <div class=""cta-container"">
                <a href=""{confirmationLink}"" class=""confirm-button"">
                    ??? Confirm Account Deletion
                </a>
            </div>
            
            <div class=""alternative-section"">
                <p style=""margin-bottom: 15px; color: #6c757d; font-weight: 600;"">
                    If the button doesn't work, copy and paste this link:
                </p>
                <div class=""link-box"">
                    {confirmationLink}
                </div>
            </div>
            
            <div style=""text-align: center; margin-top: 30px; padding: 20px; background: #e8f5e8; border-radius: 10px;"">
                <p style=""color: #2d5a2d; font-weight: 600;"">
                    Changed your mind? Simply ignore this email and your account will remain active.
                </p>
                <p style=""color: #5a6c7d; font-size: 14px; margin-top: 10px;"">
                    This deletion link expires in 24 hours for security.
                </p>
            </div>
        </div>
        
        <div class=""footer"">
            <div class=""footer-text"">
                � 2024 AuthAPI - Secure Authentication Platform<br>
                This email was sent automatically.
            </div>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, htmlBody);
        }

        public async Task SendAccountDeletedNotificationAsync(string email)
        {
            var subject = "? Account Successfully Deleted - AuthAPI";
            
            var htmlBody = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Account Deleted</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            background: linear-gradient(135deg, #2ecc71 0%, #27ae60 100%);
            padding: 20px;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #27ae60 0%, #229954 100%);
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}
        
        .logo {{
            font-size: 48px;
            margin-bottom: 10px;
        }}
        
        .header h1 {{
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 8px;
        }}
        
        .content {{
            padding: 50px 40px;
            text-align: center;
        }}
        
        .success-icon {{
            font-size: 64px;
            margin-bottom: 20px;
        }}
        
        .success-title {{
            font-size: 24px;
            color: #27ae60;
            font-weight: 700;
            margin-bottom: 20px;
        }}
        
        .description {{
            font-size: 16px;
            color: #5a6c7d;
            margin-bottom: 30px;
            line-height: 1.8;
        }}
        
        .info-box {{
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin: 25px 0;
            border-left: 4px solid #27ae60;
        }}
        
        .footer {{
            background: #2c3e50;
            color: #95a5a6;
            padding: 30px;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <div class=""logo"">?</div>
            <h1>Account Deleted</h1>
            <p>Your AuthAPI account has been permanently removed</p>
        </div>
        
        <div class=""content"">
            <div class=""success-icon"">???</div>
            <div class=""success-title"">Account Successfully Deleted</div>
            
            <div class=""description"">
                Your AuthAPI account ({email}) has been permanently deleted from our servers. All associated data has been removed as requested.
            </div>
            
            <div class=""info-box"">
                <h4 style=""color: #2c3e50; margin-bottom: 15px;"">What happened:</h4>
                <ul style=""text-align: left; color: #5a6c7d; padding-left: 20px;"">
                    <li>Your account and all personal data have been permanently deleted</li>
                    <li>All authentication tokens have been invalidated</li>
                    <li>Your email address is now available for future registration</li>
                    <li>This action cannot be undone</li>
                </ul>
            </div>
            
            <div style=""margin-top: 30px; padding: 20px; background: #e8f5e8; border-radius: 10px;"">
                <p style=""color: #2d5a2d; font-weight: 600;"">
                    Thank you for using AuthAPI. If you decide to return in the future, you're always welcome to create a new account.
                </p>
            </div>
        </div>
        
        <div class=""footer"">
            <div style=""font-size: 14px; line-height: 1.6;"">
                � 2024 AuthAPI - Secure Authentication Platform<br>
                This is the final email you will receive from us.
            </div>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, htmlBody);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                
                // From
                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? throw new InvalidOperationException("FromEmail not configured");
                var fromName = _configuration["EmailSettings:FromName"] ?? "AuthAPI";
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                
                // To
                message.To.Add(new MailboxAddress("", toEmail));
                
                // Subject
                message.Subject = subject;
                
                // Body
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };
                message.Body = bodyBuilder.ToMessageBody();

                // SMTP settings - Environment Variable primary
                var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? throw new InvalidOperationException("SmtpHost not configured");
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? throw new InvalidOperationException("SmtpUsername not configured");

                // Get the password from the Environment Variable, otherwise from appsettings
                var smtpPassword = Environment.GetEnvironmentVariable("EmailSettings__SmtpPassword") 
                    ?? _configuration["EmailSettings:SmtpPassword"] 
                    ?? throw new InvalidOperationException("SmtpPassword not configured");

                using var client = new SmtpClient();
                
                // Connect to SMTP server
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                
                // Authenticate
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                
                // Send email
                await client.SendAsync(message);
                
                // Disconnect
                await client.DisconnectAsync(true);
                
                _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                throw;
            }
        }
    }
}