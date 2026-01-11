FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN rm /etc/dpkg/dpkg.cfg.d/excludes && \
    rm -f /usr/bin/man && \
    dpkg-divert --quiet --remove /usr/bin/man && \
    apt-get update && \
    apt-get install -y --reinstall man-db && \
    apt-get install -y \
    man-db \
    manpages \
    manpages-dev && \
    apt-get install -y \
    less \
    zsh \
    git \
    curl \
    btop \
    neofetch \
    tree \
    tldr \
    locales \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://deb.nodesource.com/setup_24.x | bash - \
    && apt-get install -y nodejs

RUN locale-gen ru_RU.UTF-8
ENV LANG=ru_RU.UTF-8 \
    LANGUAGE=ru_RU:ru \
    LC_ALL=ru_RU.UTF-8

RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended \
    && sed -i 's/ZSH_THEME=.*/ZSH_THEME="robbyrussell"/' /root/.zshrc

RUN git clone --depth 1 https://github.com/junegunn/fzf.git /root/.fzf && \
    /root/.fzf/install --all

WORKDIR /workspace

SHELL ["/bin/zsh", "-c"]

CMD ["/bin/zsh"]