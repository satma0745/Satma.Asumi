using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Satma.Asumi.Web.Persistence.Entities;

public class UserSession
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    public User? User { get; init; }
    
    public required Guid RefreshTokenId { get; init; }
    
    // TODO: Introduce a background job, which will regularly delete expired sessions.
    public required DateTime ExpiresAt { get; init; }
}

public class UserSessionEntityConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> userSessionEntity)
    {
        userSessionEntity.ToTable("user_sessions");

        userSessionEntity
            .HasKey(userSession => userSession.Id)
            .HasName("pk_user_sessions");

        userSessionEntity
            .Property(userSession => userSession.Id)
            .HasColumnName("id");

        userSessionEntity
            .Property(userSession => userSession.UserId)
            .HasColumnName("user_id");

        userSessionEntity
            .Property(userSession => userSession.RefreshTokenId)
            .HasColumnName("refresh_token_id");

        userSessionEntity
            .Property(userSession => userSession.ExpiresAt)
            .HasColumnName("expires_at");

        userSessionEntity
            .HasOne(userSession => userSession.User)
            .WithMany(user => user.Sessions)
            .HasForeignKey(userSession => userSession.UserId)
            .HasConstraintName("fk_user_sessions_users_user_id")
            .IsRequired();

        userSessionEntity
            .HasIndex(userSession => userSession.UserId)
            .HasDatabaseName("ix_user_sessions_user_id");
    }
}