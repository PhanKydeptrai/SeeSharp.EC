using System;
using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using Domain.Utilities.Events.EmployeeEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers;
/// <summary>
/// Consumer xử lý việc gửi mail
/// </summary>
internal sealed class EmailNotificationConsumer
    : IConsumer<AccountVerificationEmailSentIntergrationEvent>,
    IConsumer<CustomerChangePasswordIntergrationEvent>,
    IConsumer<CustomerConfirmChangePasswordEvent>,
    IConsumer<CustomerConfirmedEvent>,
    IConsumer<CustomerResetPasswordEmailSendEvent>,
    IConsumer<CustomerResetPasswordEvent>,
    IConsumer<EmployeeChangePasswordEvent>,
    IConsumer<EmployeeConfirmChangePasswordEvent>,
    IConsumer<EmployeeResetPasswordEmailSendEvent>,
    IConsumer<EmployeeResetPasswordEvent>,
    IConsumer<SendDefaultPasswordToUserEvent>
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmailNotificationConsumer> _logger;

    public EmailNotificationConsumer(
        IFluentEmail fluentEmail,
        IOutBoxMessageServices outBoxMessageServices,
        EmailVerificationLinkFactory emailVerificationLinkFactory,
        IUnitOfWork unitOfWork,
        ILogger<EmailNotificationConsumer> logger)
    {
        _fluentEmail = fluentEmail;
        _outBoxMessageServices = outBoxMessageServices;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region AccountVerificationEmailSentIntergrationEventConsumer
    /// <summary>
    /// Consumer gửi mail xác nhận tài khoản cho khách hàng (Để kích hoạt tài khoản)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<AccountVerificationEmailSentIntergrationEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming AccountVerificationEmailSentEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận tài khoản")
                .Body($"<h1>Xin chào</h1><p>Để xác nhận tài khoản, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmailVerification(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume AccountVerificationEmailSentEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume AccountVerificationEmailSentEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed AccountVerificationEmailSentEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region CustomerChangePasswordIntergrationEventConsumer
    /// <summary>
    /// Consumer gửi mail xác nhận thay đổi mật khẩu cho khách hàng (Để xác nhận thay đổi mật khẩu)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<CustomerChangePasswordIntergrationEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            var email = _fluentEmail
            .To(message.Email)
            .Subject("Xác nhận thay đổi mật khẩu")
            .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForChangePassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region CustomerConfirmChangePasswordEventConsumer
    public async Task Consume(ConsumeContext<CustomerConfirmChangePasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu thành công")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu của bạn đã được thay đổi thành công. Nếu bạn không thực hiện thay đổi này, vui lòng liên hệ với chúng tôi ngay lập tức.</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerConfirmChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerConfirmChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion


    #region CustomerConfirmedEventConsumer
    /// <summary>
    /// Consumer gửi mail thông báo xác nhận tài khoản thành công cho khách hàng
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<CustomerConfirmedEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerConfirmedEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận tài khoản thành công")
                .Body($"<h1>Xin chào</h1><p>Tài khoản của bạn đã được xác nhận thành công. <br> Nhằm tăng trải nghiệm mua hàng của quý khách cửa hàng xin phép gửi tặng quý khách một mã giảm giá áp dụng trị giá 10K, quý khách có thể áp dụng ngay cho đơn hàng đầu tiên <br> Chúc quý khách mua hàng vui vẻ!</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();

            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerConfirmedEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerConfirmedEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerConfirmedEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region CustomerResetPasswordEmailSendEventConsumer
    /// <summary>
    /// Consumer gửi mail xác nhận reset mật khẩu cho khách hàng
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<CustomerResetPasswordEmailSendEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForResetPassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);
            
            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerResetPasswordEmailSendEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEmailSendEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region CustomerResetPasswordEventConsumer
    /// <summary>
    /// Consumer gửi mail thông báo mật khẩu mới cho khách hàng
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<CustomerResetPasswordEvent> context)
    {
        var message = context.Message;
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEvent for userId: {userId}",
            message.UserId);
        //--------------------------------------------------------------
        try
        {
            //Consume message            
            var email = _fluentEmail
            .To(message.Email)
            .Subject("Thông báo thay đổi mật khẩu")
            .Body($"<h1>Xin chào</h1><p>Mật khẩu mới của bạn là: <b>{message.RandomPassword}</b></p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerResetPasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEvent for userId: {userId}",
                message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEvent for userId: {userId}",
            message.UserId);
        //-------------------------------------------------

    }
    #endregion

    #region EmployeeChangePasswordEventConsumer
    /// <summary>
    /// Consumer gửi mail xác nhận thay đổi mật khẩu cho nhân viên
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<EmployeeChangePasswordEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmployeeChangePassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region EmployeeConfirmChangePasswordEventConsumer
    /// <summary>
    /// Consumer gửi mail thông báo xác nhận thay đổi mật khẩu thành công cho nhân viên
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<EmployeeConfirmChangePasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu thành công")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu của bạn đã được thay đổi thành công!</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeConfirmChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeConfirmChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region EmployeeResetPasswordEmailSendEventConsumer
    /// <summary>
    /// Consumer gửi mail cho nhân viên để đặt lại mật khẩu
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<EmployeeResetPasswordEmailSendEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận reset mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmployeeResetPassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);
            
            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeResetPasswordEmailSendEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeResetPasswordEmailSendEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region EmployeeResetPasswordEventConsumer
    /// <summary>
    /// Consumer gửi mail thông báo mật khẩu mới cho nhân viên
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<EmployeeResetPasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeResetPasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Mật khẩu mới của bạn")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu mới của bạn là: {message.RandomPassword}</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();

            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeResetPasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeResetPasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeResetPasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
    #endregion

    #region SendDefaultPasswordToUserEventConsumer
    public async Task Consume(ConsumeContext<SendDefaultPasswordToUserEvent> context)
    {
        var message = context.Message;
        var email = message.Email;
        var randomPassword = message.RandomPassword;
        var messageId = message.MessageId;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming SendDefaultPasswordToUserEvent for email: {email}",
            email);
        //--------------------------------------------------------------

        try
        {
            // Send the email using FluentEmail
            await _fluentEmail
                .To(email)
                .Subject("Your Default Password")
                .Body($"Your default password is: {randomPassword}")
                .SendAsync();
            
            // Update outbox message status
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                messageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                messageId,
                OutboxMessageStatus.Failed,
                "Failed to consume SendDefaultPasswordToUserEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume SendDefaultPasswordToUserEvent for email: {email}",
                email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed SendDefaultPasswordToUserEvent for email: {email}",
            email);
        //-------------------------------------------------
    }
    #endregion


}
