using Confluent.Kafka;
using RECAMAS.Application.Configuration;

namespace RECAMAS.Infrastructure.Messaging;

/// Ported verbatim from CivilianPortal. Opt-in SASL: when SecurityProtocol is
/// blank the config is left untouched (plaintext), so this is a no-op until a
/// deployment actually sets the SASL env vars.
public static class KafkaSaslConfigurator
{
    public static void ApplySasl(this ClientConfig config, KafkaSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.SecurityProtocol)) return;

        // Confluent's SecurityProtocol/SaslMechanism enums have no underscores
        // (SaslSsl, not SASL_SSL); strip before parsing.
        if (!Enum.TryParse<SecurityProtocol>(settings.SecurityProtocol.Replace("_", ""), ignoreCase: true, out var protocol))
            throw new ArgumentException("KAFKA_SECURITY_PROTOCOL is not a valid value.");
        config.SecurityProtocol = protocol;

        if (!string.IsNullOrWhiteSpace(settings.SaslMechanism))
        {
            if (!Enum.TryParse<SaslMechanism>(settings.SaslMechanism.Replace("_", ""), ignoreCase: true, out var mechanism))
                throw new ArgumentException("KAFKA_SASL_MECHANISM is not a valid value.");
            config.SaslMechanism = mechanism;
        }

        config.SaslUsername = settings.SaslUsername;
        config.SaslPassword = settings.SaslPassword;
    }
}
